namespace Incoding.MvcContrib
{
    #region << Using >>

    using System;
    using System.Linq.Expressions;

    #endregion

    public interface IConditionalBuilder 
    {
        [Obsolete("Use method native inversion bool on C#")]
        ConditionalBuilder Not { get; }

        IConditionalBinaryBuilder Is(Expression<Func<bool>> expression);

        [Obsolete("Use method Selector.JS.Call or Selector.JS.Eval in Is")]
        IConditionalBinaryBuilder Eval(string code);

        IConditionalBinaryBuilder Data<TModel>(Expression<Func<TModel, bool>> expression);
    }
}