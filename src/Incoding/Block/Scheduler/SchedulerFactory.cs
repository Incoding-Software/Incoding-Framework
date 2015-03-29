namespace Incoding.Block
{
    #region << Using >>

    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Incoding.Block.ExceptionHandling;
    using Incoding.Block.IoC;
    using Incoding.Block.Logging;
    using Incoding.CQRS;
    using Incoding.Maybe;

    #endregion

    public class SchedulerFactory
    {
        #region Static Fields

        static readonly Lazy<SchedulerFactory> instance = new Lazy<SchedulerFactory>(() => new SchedulerFactory());

        #endregion

        #region Properties

        public static SchedulerFactory Instance { get { return instance.Value; } }

        #endregion

        #region Api Methods

        public void Initialize(Action<InitScheduler> initializeAction = null)
        {
            var init = new InitScheduler();
            initializeAction.Do(action => action(init));
            Task.Factory.StartNew(() =>
                                      {
                                          try
                                          {
                                              while (init.Conditional())
                                              {
                                                  var dispatcher = IoCFactory.Instance.TryResolve<IDispatcher>();
                                                  foreach (var pair in dispatcher.Query(new GetExpectedDelayToSchedulerQuery
                                                                                            {
                                                                                                    FetchSize = init.FetchSize,
                                                                                                    Date = DateTime.UtcNow
                                                                                            }, init.Setting))
                                                  {
                                                      var ids = pair.Value.Select(r => r.Id).ToArray();
                                                      dispatcher.Push(new ChangeDelayToSchedulerStatusCommand
                                                                          {
                                                                                  Ids = ids,
                                                                                  Status = DelayOfStatus.InProgress
                                                                          }, init.Setting);

                                                      try
                                                      {
                                                          var policy = ActionPolicy.Direct();
                                                          var composite = new CommandComposite();
                                                          foreach (var delayToScheduler in pair.Value)
                                                          {
                                                              var instanceCommand = delayToScheduler.Instance;
                                                              composite.Quote(instanceCommand)
                                                                       .WithConnectionString(instanceCommand.Setting.Connection)
                                                                       .WithDateBaseString(instanceCommand.Setting.DataBaseInstance)
                                                                       .Mute(instanceCommand.Setting.Mute);
                                                          }

                                                          composite.Quote(new ChangeDelayToSchedulerStatusCommand
                                                                              {
                                                                                      Ids = ids,
                                                                                      Status = DelayOfStatus.Success,
                                                                                      UpdateNextStart = true
                                                                              }, init.Setting);
                                                          policy.Do(() => dispatcher.Push(composite));
                                                      }
                                                      catch (Exception ex)
                                                      {
                                                          dispatcher.Push(new ChangeDelayToSchedulerStatusCommand
                                                                              {
                                                                                      Ids = ids,
                                                                                      Status = DelayOfStatus.Error,
                                                                                      Description = ex.ToString()
                                                                              }, init.Setting);
                                                      }
                                                  }

                                                  ////ncrunch: no coverage start
                                                  Thread.Sleep(init.Interval);
                                              }
                                          }
                                                  ////ncrunch: no coverage end
                                          catch (Exception ex)
                                          {
                                              if (!string.IsNullOrWhiteSpace(init.Log_Debug))
                                                  LoggingFactory.Instance.LogException(init.Log_Debug, ex);
                                          }
                                      }, init.TaskCreationOptions);
        }

        #endregion
    }
}