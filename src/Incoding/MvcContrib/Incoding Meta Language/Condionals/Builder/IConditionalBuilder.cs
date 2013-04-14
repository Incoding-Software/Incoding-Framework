namespace Incoding.MvcContrib
{
    #region << Using >>

    using System;
    using System.Linq.Expressions;

    #endregion

    public interface IConditionalBuilder : IConditionalPlugInBuilder
    {
        ConditionalBuilder Not { get; }

        IConditionalBinaryBuilder Is(Expression<Func<bool>> expression);

        IConditionalBinaryBuilder Confirm(Selector selector);

        IConditionalBinaryBuilder Eval(string code);

        IConditionalBinaryBuilder Url(string url, Action<JqueryAjaxOptions> configuration = null);

        IConditionalBinaryBuilder Exist(Selector jquerySelector);

        IConditionalBinaryBuilder FormIsValid(Func<JquerySelector, JquerySelector> configuration);

        IConditionalBinaryBuilder FormIsValid(JquerySelector jquerySelector);

        IConditionalBinaryBuilder Data<TModel>(Expression<Func<TModel, bool>> expression);

        IConditionalBinaryBuilder Support(ModernizrSupport modernizrSupport);
    }
}