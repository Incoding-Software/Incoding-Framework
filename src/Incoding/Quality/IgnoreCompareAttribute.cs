namespace Incoding.Quality
{
    #region << Using >>

    using System;
    using System.Diagnostics.CodeAnalysis;
    using JetBrains.Annotations;

    #endregion

    [AttributeUsage(AttributeTargets.Property)]
    public sealed class IgnoreCompareAttribute : Attribute
    {
        #region Constructors

        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "reason", Justification = "For conciseness")]
        public IgnoreCompareAttribute([UsedImplicitly] string reason) { }

        #endregion
    }
}