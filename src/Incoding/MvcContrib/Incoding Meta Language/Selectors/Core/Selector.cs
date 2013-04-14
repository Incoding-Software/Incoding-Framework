namespace Incoding.MvcContrib
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.WebPages;
    using Incoding.Extensions;
    using Incoding.Maybe;

    #endregion

    /// <summary>
    ///     An HTML snippet, action expression, jQuery object, or DOM element specifying the structure to wrap around the matched elements.
    /// </summary>
    public partial class Selector : ISelector
    {
        #region Fields

        protected readonly List<string> methods = new List<string>();

        protected string selector;

        #endregion

        #region Constructors

        public Selector(string selector)
        {
            this.selector = selector;
        }

        #endregion

        #region Factory constructors

        public static Selector FromHelperResult(Func<object, HelperResult> text)
        {
            return text.Invoke(null)
                       .ToHtmlString()
                       .Replace(Environment.NewLine, string.Empty);
        }

        public static Selector Value(object value)
        {
            return new Selector(value.ToString());
        }

        #endregion

        #region Properties

        public static JquerySelector Jquery
        {
            get { return new JquerySelector(string.Empty); }
        }

        public static IncodingSelector Incoding
        {
            get { return new IncodingSelector(string.Empty); }
        }

        public static JavaScriptSelector JavaScript
        {
            get { return new JavaScriptSelector(); }
        }

        #endregion

        #region ISelector Members

        public string ToSelector()
        {
            return this.selector;
        }

        #endregion

        #region Api Methods

        public string ToString(bool escaping)
        {
            if (string.IsNullOrWhiteSpace(this.selector))
                return string.Empty;

            if (this is JquerySelector || this is JquerySelectorExtend)
            {
                bool isVariable = this.selector == Jquery.Self().ToSelector() || this.selector == Jquery.Document().ToSelector() || this.selector == Jquery.Target().ToSelector();
                string evalJquerySelector = isVariable ? "$({0})".F(this.selector) : "$('{0}')".F(this.selector);
                this.methods.DoEach(s => evalJquerySelector += s);
                return evalJquerySelector;
            }

            this.methods.DoEach(s => this.selector += s);

            if (this is JavaScriptDateTimeSelector)
                return this.selector;

            return escaping ? "'{0}'".F(this.selector) : this.selector;
        }

        public void AddMethod(string funcName, params object[] args)
        {
            string stringArgs = args.Aggregate(string.Empty, (res, orig) => res += "'{0}',".F(orig.ToString()));
            if (stringArgs.EndsWith(","))
                stringArgs = stringArgs.Substring(0, stringArgs.Length - 1);

            this.methods.Add(".{0}({1})".F(funcName.Replace("()", string.Empty), stringArgs));
        }

        #endregion

        protected void AndSelector(string value)
        {
            if (string.IsNullOrWhiteSpace(this.selector))
                this.selector += value;
            else
                this.selector += " " + value;
        }

        protected void OrSelector(string value)
        {
            if (string.IsNullOrWhiteSpace(this.selector))
                this.selector += value;
            else
                this.selector += "," + value;
        }

        protected void AlsoSelector(string value)
        {
            this.selector += value;
        }

        public override string ToString()
        {
            return ToString(true);
        }
    }
}