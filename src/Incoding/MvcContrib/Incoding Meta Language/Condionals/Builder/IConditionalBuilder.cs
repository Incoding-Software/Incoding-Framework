namespace Incoding.MvcContrib
{
    #region << Using >>

    using System;
    using System.Linq.Expressions;

    #endregion

    public interface IConditionalBuilder
    {
        [Obsolete("Use If with Expression")]
        ConditionalBuilder Not { get; }

        [Obsolete("Use If with Expression")]
        IConditionalBinaryBuilder Is(Expression<Func<bool>> expression);

        [Obsolete("Use If with Expression (Selector.JS.Call or Selector.JS.Eval)")]
        IConditionalBinaryBuilder Eval(string code);

        [Obsolete("Use If with Expression (Selector.Result or Selector.Result.For)")]
        IConditionalBinaryBuilder Data<TModel>(Expression<Func<TModel, bool>> expression);
    }
}