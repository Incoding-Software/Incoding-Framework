namespace Incoding.MvcContrib
{
    #region << Using >>

    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using Incoding.Extensions;
    using Incoding.Maybe;

    #endregion

    /// <summary>
    ///     An HTML snippet, action expression, jQuery object, or DOM element specifying the structure to wrap around the matched elements.
    /// </summary>
    public class JquerySelector : Selector
    {
        #region Constructors

        internal JquerySelector(string selector)
                : base(selector) { }

        #endregion

        #region Factory constructors

        public static string Escaping(string value)
        {
            return value
                    .Replace(".", "\\.")
                    .Replace("#", "\\#");
        }

        #endregion

        #region Api Methods

        public JquerySelectorExtend Custom(string custom)
        {
            AlsoSelector(custom);
            return new JquerySelectorExtend(this.selector);
        }

        /// <summary>
        ///     Selects elements that have the specified <paramref name="attribute" />, with any value.
        ///     <remarks>
        ///         Jquery $('[attribute]')
        ///     </remarks>
        /// </summary>
        public JquerySelectorExtend HasAttribute(string attribute)
        {
            AndSelector("[{0}]".F(attribute.ToLower()));
            return new JquerySelectorExtend(this.selector);
        }

        /// <summary>
        ///     Selects elements that have the specified <paramref name="attribute" />, with any value.
        ///     <remarks>
        ///         Jquery $('[attribute]')
        ///     </remarks>
        /// </summary>
        public JquerySelectorExtend HasAttribute(HtmlAttribute attribute)
        {
            return HasAttribute(attribute.ToStringLower());
        }

        /// <summary>
        ///     Selects elements that have the specified <paramref name="attribute" /> with a <paramref name="value" /> beginning exactly with a given string.
        ///     <remarks>
        ///         $('[attribute^="value"]')
        ///     </remarks>
        /// </summary>
        public JquerySelectorExtend StartWithAttribute(string attribute, string value)
        {
            AndSelector(FixedAsAttribute(attribute, Escaping(value), "^"));
            return new JquerySelectorExtend(this.selector);
        }

        /// <summary>
        ///     Selects elements that have the specified <paramref name="attribute" /> with a <paramref name="value" /> beginning exactly with a given string.
        /// </summary>
        public JquerySelectorExtend StartWithAttribute(HtmlAttribute attribute, string value)
        {
            return StartWithAttribute(attribute.ToStringLower(), value);
        }

        /// <summary>
        ///     Selects elements that have the specified <paramref name="attribute" /> with a <paramref name="value" /> containing the a given substring.
        /// </summary>
        public JquerySelectorExtend ContainsAttribute(string attribute, string value)
        {
            AndSelector(FixedAsAttribute(attribute, Escaping(value), "*"));
            return new JquerySelectorExtend(this.selector);
        }

        /// <summary>
        ///     Selects elements that have the specified <paramref name="attribute" /> with a <paramref name="value" /> containing the a given substring.
        /// </summary>
        public JquerySelectorExtend ContainsAttribute(HtmlAttribute attribute, string value)
        {
            return ContainsAttribute(attribute.ToString(), value);
        }

        /// <summary>
        ///     Selects elements that have the specified <paramref name="attribute" /> with a <paramref name="value" /> ending exactly with a given string. The comparison is case sensitive.
        /// </summary>
        public JquerySelectorExtend EndsWithAttribute(string attribute, string value)
        {
            AndSelector(FixedAsAttribute(attribute, Escaping(value), "$"));
            return new JquerySelectorExtend(this.selector);
        }

        public JquerySelectorExtend Expression(JqueryExpression expression)
        {
            foreach (var exist in Enum.GetValues(typeof(JqueryExpression))
                                      .Cast<JqueryExpression>()
                                      .Where(r => expression.HasFlag(r)))
                AlsoSelector(":{0}".F(exist.ToJqueryString()));

            return new JquerySelectorExtend(this.selector);
        }

        /// <summary>
        ///     Selects elements that have the specified <paramref name="attribute" /> with a <paramref name="value" /> ending exactly with a given string. The comparison is case sensitive.
        /// </summary>
        public JquerySelectorExtend EndsWithAttribute(HtmlAttribute attribute, string value)
        {
            return EndsWithAttribute(attribute.ToString(), value);
        }

        /// <summary>
        ///     Selects a single element with the given <paramref name="ids" /> attribute.
        /// </summary>
        public JquerySelectorExtend Id(params string[] ids)
        {
            ids.DoEach((id, index) => OrSelector("#" + Escaping(id)));
            return new JquerySelectorExtend(this.selector);
        }

        /// <summary>
        ///     Selects a single element with the given <paramref name="expression" /> attribute.
        /// </summary>
        public JquerySelectorExtend Id<TModel>(Expression<Func<TModel, object>> expression)
        {
            return Id(expression.GetMemberNameAsHtmlId());
        }

        /// <summary>
        ///     Selects a single element with the given <paramref name="name" /> attribute.
        /// </summary>
        public JquerySelectorExtend Name(string name)
        {
            return EqualsAttribute(HtmlAttribute.Name.ToString(), name);
        }

        /// <summary>
        ///     Selects a single element with the given <paramref name="expression" /> attribute.
        /// </summary>
        public JquerySelectorExtend Name<TModel>(Expression<Func<TModel, object>> expression)
        {
            return Name(expression.GetMemberName());
        }

        public JquerySelectorExtend Tag(HtmlTag tag)
        {
            AndSelector(tag.ToStringLower());
            return new JquerySelectorExtend(this.selector);
        }

        /// <summary>
        ///     Selects all elements.
        /// </summary>
        public JquerySelectorExtend All()
        {
            AndSelector("*");
            return new JquerySelectorExtend(this.selector);
        }

        /// <summary>
        ///     Selects all elements with the given <paramref name="class" />.
        /// </summary>
        public JquerySelectorExtend Class(string @class)
        {
            AndSelector("." + Escaping(@class.Trim()));
            return new JquerySelectorExtend(this.selector);
        }

        /// <summary>
        ///     Selects all elements with the given <paramref name="@class" />.
        /// </summary>
        public JquerySelectorExtend Class(params string[] @class)
        {
            @class.DoEach((r, index) => OrSelector("." + Escaping(r)));
            return new JquerySelectorExtend(this.selector);
        }

        public JquerySelectorExtend Document()
        {
            AndSelector("window.document");
            return new JquerySelectorExtend(this.selector);
        }

        public JquerySelectorExtend Immediate()
        {
            AndSelector(">");
            return new JquerySelectorExtend(this.selector);
        }

        public JquerySelectorExtend Self()
        {
            AndSelector("this.self");
            return new JquerySelectorExtend(this.selector);
        }

        public JquerySelectorExtend Target()
        {
            AndSelector("this.target");
            return new JquerySelectorExtend(this.selector);
        }

        #endregion

        #region Equals

        /// <summary>
        ///     Select elements that either don't have the specified <paramref name="attribute" />, or do have the specified
        ///     <paramref
        ///         name="attribute" />
        ///     but not with a certain <paramref name="value" />.
        /// </summary>
        public JquerySelectorExtend NotEqualsAttribute(string attribute, string value)
        {
            AndSelector(FixedAsAttribute(attribute, Escaping(value), "!"));
            return new JquerySelectorExtend(this.selector);
        }

        /// <summary>
        ///     Select elements that either don't have the specified <paramref name="attribute" />, or do have the specified
        ///     <paramref
        ///         name="attribute" />
        ///     but not with a certain <paramref name="value" />.
        /// </summary>
        public JquerySelectorExtend NotEqualsAttribute(HtmlAttribute attribute, string value)
        {
            return NotEqualsAttribute(attribute.ToString(), value);
        }

        /// <summary>
        ///     Selects elements that have the specified <c>attribute</c> with a <c>value</c> exactly equal to a certain <c>value</c>.
        /// </summary>
        public JquerySelectorExtend EqualsAttribute(string attribute, string value)
        {
            AlsoSelector(FixedAsAttribute(attribute.ToLower(), value, string.Empty));
            return new JquerySelectorExtend(this.selector);
        }

        /// <summary>
        ///     Selects elements that have the specified <c>attribute</c> with a <c>value</c> exactly equal to a certain <c>value</c>.
        /// </summary>
        public JquerySelectorExtend EqualsAttribute(HtmlAttribute attribute, string value)
        {
            return EqualsAttribute(attribute.ToString(), value);
        }

        #endregion

        static string FixedAsAttribute(string attribute, string value, string global)
        {
            return "[{0}{1}=\"{2}\"]".F(attribute.ToLower(), global, value);
        }
    }
}