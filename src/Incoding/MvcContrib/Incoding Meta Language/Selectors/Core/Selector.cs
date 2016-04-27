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

    public partial class Selector
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

        protected Selector(Selector selector)
        {
            this.selector = selector.selector;
            this.methods.AddRange(selector.methods);
        }

        #endregion

        #region Factory constructors

        [Obsolete("Use native element without wrapp", false)]
        public static Selector Value(object value)
        {
            return value.GetType().IsEnum ? new Selector((int)value) : new Selector(value.ToString());
        }

        #endregion

        #region Properties

        public static JquerySelector Jquery { get { return new JquerySelector(string.Empty); } }

        public static IncodingSelector Incoding { get { return new IncodingSelector(string.Empty); } }

        public static JavaScriptSelector JS { get { return new JavaScriptSelector(); } }

        public static EventSelector Event { get { return new EventSelector(); } }

        public static ResultSelector Result { get { return new ResultSelector(ResultSelector.TypeOfResult, string.Empty); } }

        #endregion

        #region Api Methods

        public string ToSelector()
        {
            return this.selector;
        }

        #endregion

        internal static Selector FromHelperResult(Func<object, HelperResult> text)
        {
            return text.Invoke(null)
                       .ToHtmlString()
                       .Replace(Environment.NewLine, string.Empty);
        }

        internal static string GetMethodSignature(string funcName, params object[] args)
        {
            string stringArgs = args.Aggregate(string.Empty, (res, orig) =>
                                                             {
                                                                 if (orig is Selector)
                                                                     res += "{0},".F((orig as Selector).ToString());
                                                                 else if (orig is string)
                                                                     res += "'{0}',".F(orig.ToString());
                                                                 else
                                                                     res += "{0},".F(orig.ToString());

                                                                 return res;
                                                             });

            if (stringArgs.EndsWith(","))
                stringArgs = stringArgs.Substring(0, stringArgs.Length - 1);

            return "{0}({1})".F(funcName.Replace("()", string.Empty), stringArgs);
        }

        internal void AddMethod(string funcName, params object[] args)
        {
            this.methods.Add(GetMethodSignature(funcName, args));
        }

        internal void AddProperty(string propName)
        {
            this.methods.Add("{0}".F(propName));
        }

        protected JquerySelectorExtend AndSelector(string value)
        {
            if (string.IsNullOrWhiteSpace(this.selector))
                return new JquerySelectorExtend(this.selector += value);
            if (this.selector == Jquery.Self().ToSelector() || this.selector == Jquery.Document().ToSelector() || this.selector == Jquery.Target().ToSelector())
                return new JquerySelectorExtend(this.selector).Filter(new JquerySelectorExtend(value));

            return new JquerySelectorExtend(this.selector += " " + value);
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
            if (string.IsNullOrWhiteSpace(this.selector) && !this.methods.Any())
                return string.Empty;

            if (this is JquerySelector || this is JquerySelectorExtend)
            {
                bool isVariable = this.selector == Jquery.Self().ToSelector() || this.selector == Jquery.Document().ToSelector() || this.selector == Jquery.Target().ToSelector();
                string evalJquerySelector = isVariable ? "$({0})".F(this.selector) : "$('{0}')".F(this.selector);
                this.methods.DoEach(s => evalJquerySelector += "." + s);
                return evalJquerySelector;
            }

            this.methods.DoEach(s =>
                                {
                                    if (string.IsNullOrWhiteSpace(this.selector))
                                        this.selector += s;
                                    else
                                        this.selector += "." + s;
                                });

            if (this is IJavaScriptSelector)
                return "||javascript*{0}||".F(this.selector);

            return this.selector;
        }

        public string ToJqueryObject()
        {
            if (string.IsNullOrWhiteSpace(this.selector) && !this.methods.Any())
                return string.Empty;

            bool isVariable = this.selector == Jquery.Self().ToSelector() || this.selector == Jquery.Document().ToSelector() || this.selector == Jquery.Target().ToSelector();
            if (!isVariable && !this.methods.Any())
                return "||jquery*{0}||".F(this.selector);

            return ToString();
        }
    }
}