namespace Incoding.Quality
{
    #region << Using >>

    using System;
    using System.Diagnostics.CodeAnalysis;
    using JetBrains.Annotations;

    #endregion

    ////ncrunch: no coverage start
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class IgnoreInventAttribute : Attribute
    {
        #region Constructors

        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "reason", Justification = "For conciseness")]
        public IgnoreInventAttribute([UsedImplicitly] string reason) { }

        #endregion
    }

    ////ncrunch: no coverage end
}