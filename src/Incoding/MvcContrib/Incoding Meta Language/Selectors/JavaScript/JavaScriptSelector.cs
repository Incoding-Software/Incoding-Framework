namespace Incoding.MvcContrib
{
    public class JavaScriptSelector : Selector, IJavaScriptSelector
    {
        #region Constructors

        internal JavaScriptSelector()
                : base(string.Empty) { }

        #endregion

        #region Properties

        public JSDateTimeSelector DateTime { get { return new JSDateTimeSelector("new Date()"); } }

        public JSLocationSelector Location { get { return new JSLocationSelector("window.location"); } }

        public JSGeolocationSelector Navigator { get { return new JSGeolocationSelector("navigator"); } }

        #endregion

        #region Api Methods

        public ISelector Eval(string code)
        {
            this.selector = code;
            return this;
        }

        #endregion
    }
}