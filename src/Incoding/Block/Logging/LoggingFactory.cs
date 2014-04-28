namespace Incoding.Block.Logging
{
    #region << Using >>

    using System;
    using System.Threading;
    using Incoding.Block.Core;

    #endregion

    public class LoggingFactory : FactoryBase<InitLogging>
    { 
        #region Static Fields

        static readonly Lazy<LoggingFactory> instance = new Lazy<LoggingFactory>(() => new LoggingFactory());

        #endregion

        #region Properties

        public static LoggingFactory Instance { get { return instance.Value; } }

        #endregion

        #region Api Methods

        public void LogException(string logType, Exception exception)
        {
            string message = this.init.parser.Parse(exception);
            ExecuteLog(logType, new LogMessage(message, exception, null));
        }

        public void LogMessage(string logType, string message)
        {
            ExecuteLog(logType, new LogMessage(message, null, null));
        }

        public void Log(string logType, string message, Exception exception, object state = null)
        {
            ExecuteLog(logType, new LogMessage(message, exception, state));
        }

        #endregion

        void ExecuteLog(string logType, LogMessage message)
        {
            foreach (var policy in this.init.policies)
                policy.Log(logType, message);
        }
    }
}