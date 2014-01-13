namespace Incoding.MvcContrib
{
    #region << Using >>

    using System;
    using System.Linq.Expressions;

    #endregion

    public interface IIncodingMetaLanguageCallbackBodyDsl
    {
        IIncodingMetaLanguageCallbackInstancesDsl With(Func<JquerySelector, JquerySelector> selector);

        IIncodingMetaLanguageCallbackInstancesDsl With(JquerySelector selector);

        IIncodingMetaLanguageCallbackInstancesDsl WithName<T>(Expression<Func<T, object>> memberName);

        IIncodingMetaLanguageCallbackInstancesDsl WithName(string name);

        IIncodingMetaLanguageCallbackInstancesDsl WithId<T>(Expression<Func<T, object>> memberId);

        IIncodingMetaLanguageCallbackInstancesDsl WithId(string id);

        IIncodingMetaLanguageCallbackInstancesDsl WithClass(string @class);

        IIncodingMetaLanguageCallbackInstancesDsl WithSelf(Action<JquerySelectorExtend> self);

        IIncodingMetaLanguageCallbackInstancesDsl Self();

        IIncodingMetaLanguageUtilitiesDsl Utilities { get; }
    }
}