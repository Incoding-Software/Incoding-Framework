namespace Incoding.MSpecContrib
{
    #region << Using >>

    using System;
    using System.Diagnostics.CodeAnalysis;
    using Machine.Specifications.Annotations;

    #endregion

    ////ncrunch: no coverage start
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class IgnoreFieldInventAttribute : Attribute
    {
        #region Constructors

        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "reason", Justification = "For conciseness")]
        public IgnoreFieldInventAttribute([UsedImplicitly] string reason) { }

        #endregion
    }

    ////ncrunch: no coverage end
}