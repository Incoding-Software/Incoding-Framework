namespace Incoding.MvcContrib
{
    public class JavaScriptDateTimeSelector : Selector
    {
        #region Constructors

        internal JavaScriptDateTimeSelector(string selector)
                : base(selector) { }

        #endregion

        #region Api Methods

        public JavaScriptDateTimeSelector GetDate()
        {
            AddMethod("getDate");
            return this;
        }

        public JavaScriptDateTimeSelector GetDay()
        {
            AddMethod("getDay");
            return this;
        }


        public JavaScriptDateTimeSelector GetTimezoneOffset()
        {
            AddMethod("getTimezoneOffset");
            return this;
        }


        public JavaScriptDateTimeSelector GetFullYear()
        {
            AddMethod("getFullYear");
            return this;
        }


        public JavaScriptDateTimeSelector ToTimeString()
        {
            AddMethod("toTimeString");
            return this;
        }  
        
        public JavaScriptDateTimeSelector ToDateString()
        {
            AddMethod("toDateString");
            return this;
        }

        #endregion
    }
}