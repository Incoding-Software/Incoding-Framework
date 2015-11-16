namespace Incoding.MvcContrib
{
    #region << Using >>

    using Incoding.Extensions;

    #endregion

    public class JavaScriptSelector : Selector, IJavaScriptSelector
    {

        #region Constructors

        internal JavaScriptSelector(string selector)
                : base(selector) { }


        internal JavaScriptSelector()
                : base(string.Empty) { }

        #endregion

        #region Properties

        public JSDateTimeSelector DateTime { get { return new JSDateTimeSelector("new Date()"); } }

        public JSLocationSelector Location { get { return new JSLocationSelector("window.location"); } }

        public JSGeolocationSelector Navigator { get { return new JSGeolocationSelector("navigator"); } }

        #endregion

        #region Api Methods

        public Selector Eval(string code)
        {
            this.selector = code;
            return this;
        }

        public Selector Confirm(string message)
        {
            return Eval("confirm('{0}')".F(message.ToSafeJSArgument()));
        }

        public Selector Call(string func, params object[] args)
        {
            AddMethod(func, args);
            return this;
        }

        #endregion
    }
}