namespace Incoding.Block
{
    #region << Using >>

    using System;
    using System.Diagnostics;
    using System.Threading;
    using System.Threading.Tasks;
    using Incoding.Block.IoC;
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
            Interval = new TimeSpan(0, 0, 0, 0, 10);
            TaskCreationOptions = TaskCreationOptions.LongRunning;
        }

        protected override void Execute()
        {
            Action<bool> execute = (isAsync) =>
                                   {
                                       while (true)
                                       {
                                           try
                                           {
                                               while (Conditional())
                                               {
                                                   var dispatcher = IoCFactory.Instance.TryResolve<IDispatcher>();
                                                   foreach (var response in dispatcher.Query(new GetExpectedDelayToSchedulerQuery
                                                                                             {
                                                                                                     FetchSize = FetchSize,
                                                                                                     Date = DateTime.UtcNow,
                                                                                                     Async = isAsync
                                                                                             }))
                                                   {
                                                       var closureResponse = response;

                                                       dispatcher.Push(new ChangeDelayToSchedulerStatusCommand { Id = closureResponse.Id, Status = DelayOfStatus.InProgress });

                                                       var task = Task.Factory.StartNew(() =>
                                                                             {
                                                                                 var newDispatcher = IoCFactory.Instance.TryResolve<IDispatcher>();
                                                                                 try
                                                                                 {
                                                                                     Stopwatch sw = new Stopwatch();
                                                                                     sw.Start();
                                                                                     newDispatcher.Push(closureResponse.Instance);
                                                                                     sw.Stop();
                                                                                     var description = sw.Elapsed.TotalSeconds > closureResponse.TimeOut ? "Executed in {0} sec of {1} timeout".F(sw.Elapsed.TotalSeconds, closureResponse.TimeOut) : null;
                                                                                     newDispatcher.Push(new ChangeDelayToSchedulerStatusCommand { Id = closureResponse.Id, Status = DelayOfStatus.Success, Description = description });
                                                                                 }
                                                                                 catch (Exception ex)
                                                                                 {
                                                                                     newDispatcher.Push(new ChangeDelayToSchedulerStatusCommand
                                                                                                        {
                                                                                                                Id = closureResponse.Id,
                                                                                                                Status = DelayOfStatus.Error,
                                                                                                                Description = ex.ToString()
                                                                                                        });
                                                                                 }
                                                                             }, TaskCreationOptions.LongRunning);
                                                       
                                                       if (!isAsync)
                                                       {
                                                           task.Wait(response.TimeOut);
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
                                           }
                                           Thread.Sleep(5.Seconds());
                                       }
                                   };
            Task.Factory.StartNew(() => execute(true), TaskCreationOptions);
            Task.Factory.StartNew(() => execute(false), TaskCreationOptions);
        }

        #region Properties

        public string Log_Debug { get; set; }

        public TimeSpan Interval { get; set; }

        public Func<bool> Conditional { get; set; }

        public int FetchSize { get; set; }

        public TaskCreationOptions TaskCreationOptions { get; set; }

        #endregion
    }
}