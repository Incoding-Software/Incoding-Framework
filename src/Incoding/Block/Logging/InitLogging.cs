namespace Incoding.Block.Logging
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Linq;

    #endregion

    public sealed class InitLogging
    {
        #region Fields

        internal IParserException parser;

        readonly List<LoggingPolicy> loggingPolicies = new List<LoggingPolicy>();

        #endregion

        #region Constructors

        internal InitLogging(IParserException parserException)
        {
            WithParser(parserException);
        }

        #endregion

        #region Api Methods

        /// <summary>
        ///     Add default parse exception
        /// </summary>
        /// <param name="newParserException"> </param>
        public InitLogging WithParser(IParserException newParserException)
        {
            this.parser = newParserException;
            return this;
        }

        public InitLogging WithPolicy(Func<LoggingPolicy, LoggingPolicy> action)
        {
            this.loggingPolicies.Add(action(new LoggingPolicy()));
            return this;
        }

        #endregion

        internal List<LoggingPolicy> GetLoggingPolicies(string logType)
        {
            return this.loggingPolicies
                       .Where(r => r.IsSatisfied(logType))
                       .ToList();
        }
    }
}