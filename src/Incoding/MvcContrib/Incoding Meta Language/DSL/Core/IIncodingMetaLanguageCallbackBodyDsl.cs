namespace Incoding.MvcContrib
{
    using System;

    #region << Using >>

    #endregion

    public interface IIncodingMetaLanguageCallbackBodyDsl : IIncodingMetaLanguageWithDsl
    {        
        IExecutableSetting Break { get; }

        IncodingMetaCallbackDocumentDsl Document { get; }

        IncodingMetaCallbackWindowDsl Window { get; }
    }
}