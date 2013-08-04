namespace Incoding.Block.Logging
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Incoding.Extensions;
    using Incoding.Maybe;

    #endregion

    public class LoggingPolicy
    {
        #region Fields

        readonly List<ILogger> logContexts = new List<ILogger>();

        string[] supportedTypes;

        #endregion

        #region Constructors

        public LoggingPolicy() { }

        internal LoggingPolicy(LoggingPolicy sourceLoggingPolicy, List<ILogger> loggers)
                : this(sourceLoggingPolicy.supportedTypes)
        {
            Guard.NotNull("sourceLoggingPolicy", sourceLoggingPolicy);
            Guard.NotNull("loggers", loggers);

            this.logContexts.AddRange(sourceLoggingPolicy.logContexts);
            foreach (var logger in loggers)
                this.logContexts.Add(logger);
        }

        LoggingPolicy(string[] supportedTypes)
        {
            this.supportedTypes = supportedTypes;
        }

        #endregion

        #region Api Methods

        public LoggingPolicy For(params string[] logTypes)
        {
            this.supportedTypes = logTypes;
            return this;
        }

        public LoggingPolicy Use<TLogger>(Func<TLogger> loggerFactory) where TLogger : class, ILogger
        {
            return new LoggingPolicy(this, new List<ILogger> { loggerFactory.Invoke() });
        }

        public LoggingPolicy Use(ILogger logger)
        {
            return new LoggingPolicy(this, new List<ILogger> { logger });
        }

        public LoggingPolicy UseInLine(params Action<string>[] logAction)
        {
            var lambdaLoggers = logAction.Select(logContext => new ActionLogger(logContext)).OfType<ILogger>().ToList();
            return new LoggingPolicy(this, lambdaLoggers);
        }

        #endregion

        public bool IsSatisfied(string type)
        {
            return this.supportedTypes
                       .FirstOrDefault(r => r.EqualsWithInvariant(type))
                       .Has();
        }

        public void Log(LogMessage message)
        {
            foreach (var logger in this.logContexts)
                logger.Log(message);
        }
    }
}