namespace Incoding.Block.Logging
{
    #region << Using >>

    using System;
    using System.Collections.Generic;

    #endregion

    public sealed class InitLogging
    {
        ////ncrunch: no coverage start
        #region Fields

        internal readonly List<LoggingPolicy> policies = new List<LoggingPolicy>();

        internal IParserException parser = new DefaultParserException();

        #endregion

        ////ncrunch: no coverage end
        #region Api Methods

        public InitLogging WithParser(IParserException newParserException)
        {
            this.parser = newParserException;
            return this;
        }

        public InitLogging WithPolicy(Action<ILoggingPolicyFor> action)
        {
            var policy = new LoggingPolicy();
            action(policy);
            this.policies.Add(policy);
            return this;
        }

        #endregion
    }
}