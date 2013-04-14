#region copyright

// @incoding 2011
#endregion

namespace Incoding.Block.ExceptionHandling
{
    #region << Using >>

    using System.Collections.Generic;

    #endregion

    public sealed class InitExceptionHandling
    {
        #region Fields

        readonly List<ExceptionPolicy> exceptionPolicies = new List<ExceptionPolicy>();

        #endregion

        #region Constructors

        internal InitExceptionHandling() { }

        #endregion

        #region Api Methods

        public void WithPolicy(ExceptionPolicy exceptionPolicy)
        {
            Guard.NotNull("exceptionPolicy", exceptionPolicy);
            this.exceptionPolicies.Add(exceptionPolicy);
        }

        #endregion

        internal List<ExceptionPolicy> GetPolicies()
        {
            return this.exceptionPolicies;
        }
    }
}