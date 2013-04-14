namespace Incoding
{
    #region << Using >>

    using System;
    using System.Diagnostics;

    #endregion

    ////ncrunch: no coverage start

    /// <summary>
    ///     Enables writing abbreviations for contracts that get copied to other methods
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false), Conditional("CONTRACTS_FULL")]
    // ReSharper disable UnusedMember.Global
    internal sealed class ContractAbbreviateAttribute : Attribute { }

    // ReSharper restore UnusedMember.Global

    ////ncrunch: no coverage end
}