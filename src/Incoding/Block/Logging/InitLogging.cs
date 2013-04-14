namespace Incoding.Block.Logging
{
    #region << Using >>

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

        /// <summary>
        ///     Add global logging <paramref name="policy" />
        /// </summary>
        /// <param name="policy">
        ///     See <see cref="LoggingPolicy" />
        /// </param>
        public InitLogging WithPolicy(LoggingPolicy policy)
        {
            Guard.NotNull("policy", policy);

            this.loggingPolicies.Add(policy);
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