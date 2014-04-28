namespace Incoding.Block.Logging
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Incoding.Extensions;

    #endregion

    public interface ILoggingPolicyFor
    {
        ILoggingPolicyUse For(params string[] logTypes);
    }

    public interface ILoggingPolicyUse
    {
        void Use(ILogger logger);

        void UseInLine(Action<string> evaluated);
    }

    public class LoggingPolicy : ILoggingPolicyFor, ILoggingPolicyUse
    {
        #region Fields

        readonly List<ILogger> logContexts = new List<ILogger>();

        string[] supportedTypes;

        #endregion

        #region ILoggingPolicyFor Members

        public ILoggingPolicyUse For(string[] logTypes)
        {
            this.supportedTypes = logTypes;
            return this;
        }


        public ILoggingPolicyUse For(string logType)
        {
            return For(new[] { logType });
        }

        #endregion

        #region ILoggingPolicyUse Members

        public void Use(ILogger logger)
        {
            this.logContexts.Add(logger);
        }

        public void UseInLine(Action<string> evaluated)
        {
            Use(new ActionLogger(evaluated));
        }

        #endregion

        #region Api Methods

        public void Log(string type, LogMessage message)
        {
            if (!this.supportedTypes.Any(r => r.EqualsWithInvariant(type)))
                return;

            foreach (var logger in this.logContexts)
                logger.Log(message);
        }

        #endregion
    }
}