namespace Incoding.MvcContrib
{
    using System;

    #region << Using >>

    #endregion

    public interface IIncodingMetaLanguageCallbackBodyDsl : IIncodingMetaLanguageWithDsl
    {
        [Obsolete("On next version Utitlites will be remove")]
        IIncodingMetaLanguageUtilitiesDsl Utilities { get; }

        IExecutableSetting Break { get; }

        IncodingMetaCallbackDocumentDsl Document { get; }

        IncodingMetaCallbackWindowDsl Window { get; }
    }
}