namespace Incoding
{
    #region << Using >>

    using System;

    #endregion

    [Serializable]
    public class IncFrameworkException : Exception
    {
        #region Constructors

        public IncFrameworkException(string message)
                : base(message) { }

        #endregion
    }
}