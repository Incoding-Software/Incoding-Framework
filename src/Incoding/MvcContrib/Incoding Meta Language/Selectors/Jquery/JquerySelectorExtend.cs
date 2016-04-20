namespace Incoding.MvcContrib
{
    #region << Using >>

    using System;

    #endregion

    public class JquerySelectorExtend : JquerySelector
    {
    

        #region Constructors

        internal JquerySelectorExtend(string selector)
                : base(selector) { }

        internal JquerySelectorExtend(JquerySelector selector)
                : base(selector) { }

        #endregion

        #region Api Methods

        [Obsolete(@"Please use chain method like are Id(""1"").Id(""2""))")]
        public JquerySelectorExtend Also(Func<JquerySelector, JquerySelector> action = null)
        {
            AlsoSelector(action(Jquery).ToSelector());
            return this;
        }

        [Obsolete(@"Please use override method with array like are Class(""1"",""2"")")]
        public JquerySelectorExtend Or(Func<JquerySelector, JquerySelector> action = null)
        {
            OrSelector(action(Jquery).ToSelector());
            return this;
        }

        #endregion
    }
}