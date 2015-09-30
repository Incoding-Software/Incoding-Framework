namespace Incoding.Block
{
    #region << Using >>

    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
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
                                      bool finished = false;
                                      while (!finished)
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

                                                      try
                                                      {
                                                          dispatcher.Push(composite =>
                                                                          {
                                                                              composite.Quote(new ChangeDelayToSchedulerStatusCommand { Ids = ids, Status = DelayOfStatus.InProgress }, init.Setting);
                                                                              foreach (var delayToScheduler in pair.Value)
                                                                                  composite.Quote(delayToScheduler.Instance);

                                                                              composite.Quote(new ChangeDelayToSchedulerStatusCommand { Ids = ids, Status = DelayOfStatus.Success, }, init.Setting);
                                                                          });
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

                                                  Thread.Sleep(init.Interval);
                                              }
                                          }
                                          catch (Exception ex)
                                          {
                                              if (ex is ThreadAbortException)
                                                  Thread.ResetAbort(); // cancel any abort to prevent stop scheduler

                                              if (!string.IsNullOrWhiteSpace(init.Log_Debug))
                                                  LoggingFactory.Instance.LogException(init.Log_Debug, ex);
                                              finished = true;
                                          }
                                      }
                                  }, init.TaskCreationOptions);
        }

        #endregion
    }
}