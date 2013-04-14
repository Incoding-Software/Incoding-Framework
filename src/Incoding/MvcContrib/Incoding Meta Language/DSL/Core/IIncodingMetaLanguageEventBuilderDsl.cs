namespace Incoding.MvcContrib
{
    #region << Using >>

    using System;
    using System.Linq.Expressions;
    using System.Web.Routing;

    #endregion

    public interface IIncodingMetaLanguageEventBuilderDsl
    {
        IIncodingMetaLanguageEventBuilderDsl Where<TModel>(Expression<Func<TModel, bool>> expression);

        IIncodingMetaLanguageEventBuilderDsl OnSuccess(Action<IIncodingMetaLanguageCallbackBodyDsl> action);

        IIncodingMetaLanguageEventBuilderDsl OnError(Action<IIncodingMetaLanguageCallbackBodyDsl> action);

        IIncodingMetaLanguageEventBuilderDsl OnComplete(Action<IIncodingMetaLanguageCallbackBodyDsl> action);

        IIncodingMetaLanguageEventBuilderDsl OnBegin(Action<IIncodingMetaLanguageCallbackBodyDsl> action);

        IIncodingMetaLanguageEventBuilderDsl OnBreak(Action<IIncodingMetaLanguageCallbackBodyDsl> action);

        RouteValueDictionary AsHtmlAttributes(object htmlAttributes = null);

        string AsStringAttributes(object htmlAttributes = null);

        IIncodingMetaLanguageBindingDsl When(JqueryBind nextBind);

        IIncodingMetaLanguageBindingDsl When(string nextBind);
    }
}