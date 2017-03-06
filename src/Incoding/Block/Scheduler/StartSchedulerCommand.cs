namespace Incoding.Block
{
    #region << Using >>

    using System;
    using System.Diagnostics;
    using System.Threading;
    using System.Threading.Tasks;
    using Incoding.Block.Logging;
    using Incoding.CQRS;
    using Incoding.Extensions;

    #endregion

    public class StartSchedulerCommand : CommandBase
    {
        public StartSchedulerCommand()
        {
            Conditional = () => true;
            FetchSize = 10;
            Interval = new TimeSpan(0, 0, 0, 1, 0);
            TaskCreationOptions = TaskCreationOptions.LongRunning;
            DelayToStart = TimeSpan.Zero;
        }

        protected override void Execute()
        {
            Action<bool> execute = (isAsync) =>
                                   {
                                       if (DelayToStart.GetValueOrDefault(TimeSpan.Zero) != TimeSpan.Zero)
                                           Thread.Sleep(DelayToStart.GetValueOrDefault());

                                       var isFirstTime = true;
                                       while (true)
                                       {
                                           try
                                           {
                                               while (Conditional())
                                               {
                                                   foreach (var response in Dispatcher.New().Query(new GetExpectedDelayToSchedulerQuery
                                                                                                   {
                                                                                                           FetchSize = FetchSize,
                                                                                                           Date = DateTime.UtcNow,
                                                                                                           Async = isAsync,
                                                                                                           IncludeInProgress = isFirstTime
                                                                                                   }))
                                                   {
                                                       var closureResponse = response;

                                                       Dispatcher.New().Push(new ChangeDelayToSchedulerStatusCommand { Id = closureResponse.Id, Status = DelayOfStatus.InProgress });

                                                       var task = Task.Factory.StartNew(() =>
                                                                                        {
                                                                                            try
                                                                                            {
                                                                                                Stopwatch sw = new Stopwatch();
                                                                                                sw.Start();
                                                                                                Dispatcher.New().Push(closureResponse.Instance);
                                                                                                sw.Stop();

                                                                                                Dispatcher.New().Push(new ChangeDelayToSchedulerStatusCommand
                                                                                                                      {
                                                                                                                              Id = closureResponse.Id,
                                                                                                                              Status = DelayOfStatus.Success,
                                                                                                                              Description = "Executed in {0} sec of {1} timeout".F(sw.Elapsed.TotalSeconds, closureResponse.TimeOut)
                                                                                                                      });
                                                                                            }
                                                                                            catch (Exception ex)
                                                                                            {
                                                                                                if (!string.IsNullOrWhiteSpace(Log_Debug))
                                                                                                    LoggingFactory.Instance.LogException(Log_Debug, ex);

                                                                                                Dispatcher.New().Push(new ChangeDelayToSchedulerStatusCommand
                                                                                                                      {
                                                                                                                              Id = closureResponse.Id,
                                                                                                                              Status = DelayOfStatus.Error,
                                                                                                                              Description = ex.ToString()
                                                                                                                      });
                                                                                            }
                                                                                        }, TaskCreationOptions.LongRunning);

                                                       if (!isAsync)
                                                           task.Wait(response.TimeOut);
                                                   }
                                                   isFirstTime = false;
                                                   GetExpectedDelayToSchedulerQuery.LastDate = Dispatcher.New().Query(new GetExpectedDelayToSchedulerQuery.GetLastDateQuery()
                                                                                                                      {
                                                                                                                              Date = DateTime.UtcNow,
                                                                                                                              Async = true
                                                                                                                      });
                                                   Thread.Sleep(Interval);
                                               }
                                           }
                                           catch (Exception ex)
                                           {
                                               if (ex is ThreadAbortException)
                                                   Thread.ResetAbort(); // cancel any abort to prevent stop scheduler

                                               if (!string.IsNullOrWhiteSpace(Log_Debug))
                                                   LoggingFactory.Instance.LogException(Log_Debug, ex);
                                           }
                                           Thread.Sleep(5.Seconds());
                                       }
                                   };

            Task.Factory.StartNew(() => execute(true), TaskCreationOptions);
            Task.Factory.StartNew(() => execute(false), TaskCreationOptions);
        }

        #region Properties

        public TimeSpan? DelayToStart { get; set; }

        public string Log_Debug { get; set; }

        public TimeSpan Interval { get; set; }

        public Func<bool> Conditional { get; set; }

        public int FetchSize { get; set; }

        public TaskCreationOptions TaskCreationOptions { get; set; }

        #endregion
    }
}