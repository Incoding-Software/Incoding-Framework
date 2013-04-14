namespace Incoding.MvcContrib
{
    #region << Using >>

    using System;

    #endregion

    public interface IIncodingMetaLanguageCallbackBodyDsl
    {
        IIncodingMetaLanguageCallbackInstancesDsl With(Func<JquerySelector, JquerySelector> selector);

        IIncodingMetaLanguageCallbackInstancesDsl With(JquerySelector selector);

        IIncodingMetaLanguageCallbackInstancesDsl Self();

        IIncodingMetaLanguageUtilitiesDsl Utilities { get; }
    }
}