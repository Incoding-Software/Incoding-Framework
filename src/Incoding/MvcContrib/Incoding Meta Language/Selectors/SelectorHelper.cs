namespace Incoding.MvcContrib
{
    #region << Using >>

    using System;
    using System.Linq.Expressions;

    #endregion

    public class SelectorHelper<TModel>
    {
        #region Api Methods

        public JquerySelectorExtend Name<TProperty>(Expression<Func<TModel, TProperty>> property)
        {
            return Selector.Jquery.Name<TModel,TProperty>(property);
        }

        public JquerySelectorExtend Id(Expression<Func<TModel, object>> property)
        {
            return Selector.Jquery.Id(property);
        }

        public Selector QueryString(Expression<Func<TModel, object>> property)
        {
            return Selector.Incoding.QueryString(property);
        }

        public Selector HashQueryString(Expression<Func<TModel, object>> property)
        {
            return Selector.Incoding.HashQueryString(property);
        }

        public Selector Cookie(Expression<Func<TModel, object>> property)
        {
            return Selector.Incoding.Cookie(property);
        }

        #endregion
    }
}