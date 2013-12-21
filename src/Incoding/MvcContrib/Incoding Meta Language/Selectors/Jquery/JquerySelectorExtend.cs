namespace Incoding.MvcContrib
{
    #region << Using >>

    using System;
    using Incoding.Extensions;
    using System.Linq;

    #endregion

    public class JquerySelectorExtend : JquerySelector
    {
        #region Constructors

        internal JquerySelectorExtend(string selector)
                : base(selector) { }

        #endregion

        #region Api Methods


        public JquerySelectorExtend Also(Func<JquerySelector, JquerySelector> action = null)
        {
            AlsoSelector(action(Jquery).ToSelector());
            return this;
        }

        public JquerySelectorExtend Or(Func<JquerySelector, JquerySelector> action = null)
        {
            OrSelector(action(Jquery).ToSelector());
            return this;
        }

        #endregion
    }
}