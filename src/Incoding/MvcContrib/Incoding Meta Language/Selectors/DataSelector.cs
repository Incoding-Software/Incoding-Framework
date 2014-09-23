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
                : base("||data||") { }

        #endregion

        public ResultSelector For<T>(Expression<Func<T, object>> property)
        {
            return For(property.GetMemberName());
        }

        public ResultSelector For(string property)
        {
            this.selector = "||result*{0}||".F(property);
            return this;
        }

    }
}