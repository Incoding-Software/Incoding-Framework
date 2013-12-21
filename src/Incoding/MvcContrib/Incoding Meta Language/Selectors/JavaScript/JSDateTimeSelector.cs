namespace Incoding.MvcContrib
{
    public class JSDateTimeSelector : Selector, IJavaScriptSelector
    {
        #region Constructors

        internal JSDateTimeSelector(Selector selector)
                : base(selector) { }

        #endregion

        #region Api Methods

        public Selector GetDate()
        {
            var res = new JSDateTimeSelector(this);
            res.AddMethod("getDate");
            return res;
        }


        public Selector GetTime()
        {
            var res = new JSDateTimeSelector(this);
            res.AddMethod("getTime");
            return res;
        }

        public Selector GetDay()
        {
            var res = new JSDateTimeSelector(this);
            res.AddMethod("getDay");
            return res;
        }

        public Selector GetTimezoneOffset()
        {
            var res = new JSDateTimeSelector(this);
            res.AddMethod("getTimezoneOffset");
            return res;
        }

        public Selector GetFullYear()
        {
            var res = new JSDateTimeSelector(this);
            res.AddMethod("getFullYear");
            return res;
        }

        public Selector ToTimeString()
        {
            var res = new JSDateTimeSelector(this);
            res.AddMethod("toTimeString");
            return res;
        }

        public Selector ToDateString()
        {
            var res = new JSDateTimeSelector(this);
            res.AddMethod("toDateString");
            return res;
        }

        #endregion
    }
}