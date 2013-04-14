namespace Incoding
{
    #region << Using >>

    using System;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;

    #endregion

    ////ncrunch: no coverage start

    /// <summary>
    ///     Allows setting contract and tool options at assembly, type, or method granularity.
    /// </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = false), Conditional("CONTRACTS_FULL")]
    // ReSharper disable UnusedMember.Global
    internal sealed class ContractOptionAttribute : Attribute
    {
        // ReSharper restore UnusedMember.Global
        #region Constructors

        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "category", Justification = "Build-time only attribute"), SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "setting", Justification = "Build-time only attribute"), SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "toggle", Justification = "Build-time only attribute")]
        public ContractOptionAttribute(string category, string setting, bool toggle) { }

        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "category", Justification = "Build-time only attribute"), SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "setting", Justification = "Build-time only attribute"), SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "value", Justification = "Build-time only attribute")]
        public ContractOptionAttribute(string category, string setting, string value) { }

        #endregion
    }

    ////ncrunch: no coverage end
}