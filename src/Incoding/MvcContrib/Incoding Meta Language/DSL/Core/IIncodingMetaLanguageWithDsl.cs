namespace Incoding.MvcContrib
{
    using System;
    using System.Linq.Expressions;

    public interface IIncodingMetaLanguageWithDsl
    {
        IIncodingMetaLanguageCallbackInstancesDsl With(Func<JquerySelector, JquerySelectorExtend> selector);

        IIncodingMetaLanguageCallbackInstancesDsl With(JquerySelectorExtend selector);

        IIncodingMetaLanguageCallbackInstancesDsl WithName<T>(Expression<Func<T, object>> memberName);

        IIncodingMetaLanguageCallbackInstancesDsl WithName(string name);

        IIncodingMetaLanguageCallbackInstancesDsl WithId<T>(Expression<Func<T, object>> memberId);

        IIncodingMetaLanguageCallbackInstancesDsl WithId(string id);

        IIncodingMetaLanguageCallbackInstancesDsl WithClass(string @class);

        IIncodingMetaLanguageCallbackInstancesDsl WithSelf(Action<JquerySelectorExtend> self);

        IIncodingMetaLanguageCallbackInstancesDsl Self();
    }
}