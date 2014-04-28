#region copyright

// @incoding 2011
#endregion

namespace Incoding.Block.ExceptionHandling
{
    #region << Using >>

    using System;
    using System.Collections.Generic;

    #endregion

    public sealed class InitExceptionHandling
    {
        #region Fields

        readonly List<ExceptionPolicy> exceptionPolicies = new List<ExceptionPolicy>();

        #endregion

        #region Api Methods

        public void WithPolicy(Func<ExceptionPolicy, ExceptionPolicy> func)
        {
            this.exceptionPolicies.Add(func(new ExceptionPolicy()));
        }

        #endregion

        internal List<ExceptionPolicy> GetPolicies()
        {
            return this.exceptionPolicies;
        }
    }
}