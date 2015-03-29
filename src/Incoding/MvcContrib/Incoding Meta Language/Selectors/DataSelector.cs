namespace Incoding.MvcContrib
{
    #region << Using >>

    using System;
    using System.Linq.Expressions;
    using Incoding.Extensions;

    #endregion

    public class ResultSelector : Selector
    {
        #region Constructors

        internal ResultSelector()
                : base("||result||") { }

        #endregion

        #region Api Methods

        public ResultSelector For<T>(Expression<Func<T, object>> property)
        {
            return For(property.GetMemberName());
        }

        public ResultSelector For(string property)
        {
            selector = "||result*{0}||".F(property);
            return this;
        }

        #endregion
    }
}