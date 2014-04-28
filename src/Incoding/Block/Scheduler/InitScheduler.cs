namespace Incoding.Block
{
    using System;
    using System.Threading.Tasks;
    using Incoding.CQRS;
    using Incoding.Extensions;

    public class InitScheduler
    {
        #region Constructors

        public InitScheduler()
        {
            this.FetchSize = 15;
            this.Conditional = () => true;
            this.Interval = 5.Minutes();
            this.TaskCreationOptions = TaskCreationOptions.LongRunning;
        }

        #endregion

        #region Properties

        public string Log_Trace { get; set; }

        public string Log_Debug { get; set; }

        public TimeSpan Interval { get; set; }

        public Func<bool> Conditional { get; set; }

        public int FetchSize { get; set; }

        public MessageExecuteSetting Setting { get; set; }

        public TaskCreationOptions TaskCreationOptions { get; set; }

        #endregion
    }
}