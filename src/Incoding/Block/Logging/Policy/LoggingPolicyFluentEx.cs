namespace Incoding.Block.Logging
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Linq;

    #endregion

    public static class LoggingPolicyFluentEx
    {
        #region Factory constructors

        public static LoggingPolicy Use<TLogger>(this LoggingPolicy source, Func<TLogger> loggerFactory) where TLogger : class, ILogger
        {
            Guard.NotNull("loggerFactory", loggerFactory);
            return new LoggingPolicy(source, new List<ILogger> { loggerFactory.Invoke() });
        }

        public static LoggingPolicy Use(this LoggingPolicy source, ILogger logger)
        {
            Guard.NotNull("logger", logger);
            return new LoggingPolicy(source, new List<ILogger> { logger });
        }

        public static LoggingPolicy UseInLine(this LoggingPolicy source, params Action<string>[] logAction)
        {
            Guard.NotNull("logAction", logAction);

            var lambdaLoggers = logAction.Select(logContext => new ActionLogger(logContext)).OfType<ILogger>().ToList();
            return new LoggingPolicy(source, lambdaLoggers);
        }

        #endregion
    }
}