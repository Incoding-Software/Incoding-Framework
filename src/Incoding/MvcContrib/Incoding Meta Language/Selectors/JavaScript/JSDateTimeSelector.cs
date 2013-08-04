namespace Incoding.MvcContrib
{
    public class JSDateTimeSelector : Selector, IJavaScriptSelector
    {
        #region Constructors

        internal JSDateTimeSelector(Selector selector)
                : base(selector) { }

        #endregion

        #region Api Methods

        public ISelector GetDate()
        {
            var res = new JSDateTimeSelector(this);
            res.AddMethod("getDate");
            return res;
        }

        public ISelector GetDay()
        {
            var res = new JSDateTimeSelector(this);
            res.AddMethod("getDay");
            return res;
        }

        public ISelector GetTimezoneOffset()
        {
            var res = new JSDateTimeSelector(this);
            res.AddMethod("getTimezoneOffset");
            return res;
        }

        public ISelector GetFullYear()
        {
            var res = new JSDateTimeSelector(this);
            res.AddMethod("getFullYear");
            return res;
        }

        public ISelector ToTimeString()
        {
            var res = new JSDateTimeSelector(this);
            res.AddMethod("toTimeString");
            return res;
        }

        public ISelector ToDateString()
        {
            var res = new JSDateTimeSelector(this);
            res.AddMethod("toDateString");
            return res;
        }

        #endregion
    }
}