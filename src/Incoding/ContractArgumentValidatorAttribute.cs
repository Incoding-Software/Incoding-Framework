//  Include this file in your project if your project uses
//  ContractArgumentValidator or ContractAbbreviator methods

// ReSharper disable CheckNamespace
namespace Incoding
{
    // ReSharper restore CheckNamespace
    #region << Using >>

    using System;
    using System.Diagnostics;

    #endregion

    /// <summary>
    ///     Enables factoring legacy if-then-<c>throw</c> into separate methods for reuse and full control over
    ///     thrown exception and arguments
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false), Conditional("CONTRACTS_FULL")]
    internal sealed class ContractArgumentValidatorAttribute : Attribute { }
}