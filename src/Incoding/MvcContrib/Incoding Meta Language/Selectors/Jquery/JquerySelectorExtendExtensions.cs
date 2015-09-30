namespace Incoding.MvcContrib
{
    #region << Using >>

    using System;
    using Incoding.Extensions;

    #endregion

    public static class JquerySelectorExtendExtensions
    {
        #region Factory constructors

        /// <summary>
        ///     Select the element at <paramref name="index" /> n within the matched set.
        /// </summary>
        public static JquerySelectorExtend Eq(this JquerySelectorExtend selector, int index)
        {
            return selector.Custom(":eq({0})".F(index));
        }

        /// <summary>
        ///     Select all elements at an <paramref name="index" /> greater than <paramref name="index" /> within the matched set.
        /// </summary>
        public static JquerySelectorExtend Gt(this JquerySelectorExtend selector, int index)
        {
            return selector.Custom(":gt({0})".F(index));
        }

        /// <summary>
        ///     Selects elements which contain at least one element that matches the specified <paramref name="hasAction" />
        /// </summary>
        public static JquerySelectorExtend Has(this JquerySelectorExtend selector, Func<JquerySelector, JquerySelector> hasAction)
        {
            var hasSelector = Selector.Jquery;
            hasAction(hasSelector);
            return selector.Custom(":has({0})".F(hasSelector.ToSelector()));
        }

        /// <summary>
        ///     Select all elements at an <paramref name="index" /> less than <paramref name="index" /> within the matched set.
        /// </summary>
        public static JquerySelectorExtend It(this JquerySelectorExtend selector, int index)
        {
            return selector.Custom(":it({0})".F(index));
        }

        public static JquerySelectorExtend Last(this JquerySelectorExtend selector)
        {
            return selector.Expression(JqueryExpression.Last);
        }


        public static JquerySelectorExtend Visible(this JquerySelectorExtend selector)
        {
            return selector.Expression(JqueryExpression.Visible);
        }

        #endregion
    }
}