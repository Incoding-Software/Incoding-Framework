namespace Incoding.Block.Logging
{
    #region << Using >>

    using System;
    using Incoding.Block.Core;

    #endregion

    public class LoggingFactory : FactoryBase<InitLogging>
    {
        ////ncrunch: no coverage start
        #region Static Fields

        static readonly object lockObject = new object();

        ////ncrunch: no coverage end
        static volatile LoggingFactory instance;

        #endregion

        #region Constructors

        public LoggingFactory()
        {
            UnInitialize();
        }

        #endregion

        #region Properties

        public static LoggingFactory Instance
        {
            ////ncrunch: no coverage start
            get
            {
                if (instance == null)
                {
                    lock (lockObject)
                    {
                        if (instance == null)
                            instance = new LoggingFactory();
                    }
                }

                return instance;
            }

            ////ncrunch: no coverage end  
        }

        #endregion

        #region Api Methods

        public void LogException(string logType, Exception exception)
        {
            Guard.NotNull("exception", exception, "LogginFactory.Instance.LogException.Exception can't be null.");

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
            Guard.NotNull("logType", logType);
            Guard.NotNull("message", message);

            this.init
                .GetLoggingPolicies(logType)
                .ForEach(policy => policy.Log(message));
        }

        public override void UnInitialize()
        {
            this.init = new InitLogging(new DefaultParserException());
        }
    }
}