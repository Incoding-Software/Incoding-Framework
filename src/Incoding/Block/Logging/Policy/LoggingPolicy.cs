namespace Incoding.Block.Logging
{
    #region << Using >>

    using System.Collections.Generic;
    using System.Linq;
    using Incoding.Extensions;
    using Incoding.Maybe;

    #endregion

    public class LoggingPolicy
    {
        #region Fields

        readonly List<ILogger> logContexts = new List<ILogger>();

        readonly string[] supportedTypes;

        #endregion

        #region Constructors

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

        #region Factory constructors

        public static LoggingPolicy For(params string[] logTypes)
        {
            return new LoggingPolicy(logTypes);
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