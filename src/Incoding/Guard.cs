namespace Incoding
{
    #region << Using >>

    using System;
    using System.Diagnostics.Contracts;
    using Incoding.Extensions;
    using JetBrains.Annotations;

    #endregion

    public static class Guard
    {
        #region Factory constructors

        [ContractArgumentValidator]
        public static void IsConditional(string argument, [UsedImplicitly] bool conditional, string errorMessage = "Value {0} can't be not conditional")
        {
            if (!conditional)
                throw new ArgumentException(errorMessage.F(argument), argument);
            Contract.EndContractBlock();
        }

        [ContractArgumentValidator]
        public static void NotNull<T>(string argument, [UsedImplicitly] T value, string errorMessage = "Argument {0} can't be null")
        {
            if (value == null)
                throw new ArgumentException(errorMessage.F(argument), argument);
            Contract.EndContractBlock();
        }

        [ContractArgumentValidator]
        public static void NotNullOrEmpty(string argument, [UsedImplicitly] string value, string errorMessage = "Value {0} can't be null or empty")
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException(errorMessage.F(argument), argument);
            Contract.EndContractBlock();
        }

        [ContractArgumentValidator]
        public static void NotNullOrWhiteSpace(string argument, [UsedImplicitly] string value, string errorMessage = "Value {0} can't be null or white space")
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException(errorMessage.F(argument), argument);
            Contract.EndContractBlock();
        }

        #endregion
    }
}