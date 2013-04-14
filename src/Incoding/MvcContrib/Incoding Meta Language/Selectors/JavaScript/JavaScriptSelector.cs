namespace Incoding.MvcContrib
{
    public class JavaScriptSelector : Selector
    {
        #region Constructors

        internal JavaScriptSelector()
                : base(string.Empty) { }

        #endregion

        #region Properties

        public JavaScriptDateTimeSelector DateTime
        {
            get { return new JavaScriptDateTimeSelector("new Date()"); }
        }

        #endregion
    }
}