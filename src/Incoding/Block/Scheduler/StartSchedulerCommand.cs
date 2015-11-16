namespace Incoding.Block
{
    #region << Using >>

    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Incoding.Block.Logging;
    using Incoding.CQRS;

    #endregion

    public class StartSchedulerCommand : CommandBase
    {
        #region Properties

        public string Log_Debug { get; set; }

        public TimeSpan Interval { get; set; }

        public Func<bool> Conditional { get; set; }

        public int FetchSize { get; set; }

        public TaskCreationOptions TaskCreationOptions { get; set; }

        #endregion

        protected override void Execute()
        {
            Task.Factory.StartNew(() =>
                                  {
                                      bool finished = false;
                                      while (!finished)
                                      {
                                          try
                                          {
                                              while (Conditional())
                                              {
                                                  foreach (var response in Dispatcher.Query(new GetExpectedDelayToSchedulerQuery
                                                                                            {
                                                                                                    FetchSize = FetchSize, 
                                                                                                    Date = DateTime.UtcNow
                                                                                            }))
                                                  {
                                                      try
                                                      {
                                                          Dispatcher.Push(new ChangeDelayToSchedulerStatusCommand { Id = response.Id, Status = DelayOfStatus.InProgress });
                                                          Dispatcher.Push(response.Instance);
                                                          Dispatcher.Push(new ChangeDelayToSchedulerStatusCommand { Id = response.Id, Status = DelayOfStatus.Success, });
                                                      }
                                                      catch (Exception ex)
                                                      {
                                                          Dispatcher.Push(new ChangeDelayToSchedulerStatusCommand
                                                                          {
                                                                                  Id = response.Id, 
                                                                                  Status = DelayOfStatus.Error, 
                                                                                  Description = ex.ToString()
                                                                          });
                                                      }
                                                  }

                                                  Thread.Sleep(Interval);
                                              }
                                          }
                                          catch (Exception ex)
                                          {
                                              if (ex is ThreadAbortException)
                                                  Thread.ResetAbort(); // cancel any abort to prevent stop scheduler

                                              if (!string.IsNullOrWhiteSpace(Log_Debug))
                                                  LoggingFactory.Instance.LogException(Log_Debug, ex);
                                              finished = true;
                                          }
                                      }
                                  }, TaskCreationOptions);
        }
    }
}