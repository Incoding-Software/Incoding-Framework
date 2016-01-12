namespace Incoding.MvcContrib
{
    #region << Using >>

    using System;
    using Incoding.Extensions;

    #endregion

    public static class JquerySelectorExtendValueExtensions
    {
        #region Factory constructors

        /// <summary>
        ///     Get the value of an attribute for the first element in the set of matched elements every matched element.
        /// </summary>
        public static JquerySelectorExtend Attr(this JquerySelectorExtend selector, HtmlAttribute attr)
        {
            return selector.Attr(attr.ToJqueryString());
        }

        /// <summary>
        ///     Get the value of an attribute for the first element in the set of matched elements every matched element.
        /// </summary>
        public static JquerySelectorExtend Attr(this JquerySelectorExtend selector, string attr)
        {
            return selector.Method("attr", attr);
        }

        /// <summary>
        ///     Get the value of a style property for the first element in the set of matched elements every matched element.
        /// </summary>
        public static JquerySelectorExtend Css(this JquerySelectorExtend selector, CssStyling css)
        {
            return Css(selector, css.ToJqueryString());
        }

        /// <summary>
        ///     Get the value of a style property for the first element in the set of matched elements every matched element.
        /// </summary>
        public static JquerySelectorExtend Css(this JquerySelectorExtend selector, string css)
        {
            return selector.Method("css", css);
        }

        /// <summary>
        ///     Get the value of an attribute for the first element in the set of matched elements every matched element.
        /// </summary>
        public static JquerySelectorExtend FormIsValid(this JquerySelectorExtend selector)
        {
            return selector.Method("valid");
        }

        /// <summary>
        ///     Get the determine whether any of the matched elements are assigned the given class.
        /// </summary>
        public static JquerySelectorExtend HasClass(this JquerySelectorExtend selector, string @class)
        {
            return selector.Method("hasClass", Selector.Jquery.Class(@class).ToSelector().Replace(".", string.Empty));
        }

        /// <summary>
        ///     Get the determine whether any of the matched elements are assigned the given class.
        /// </summary>
        public static JquerySelectorExtend HasClass(this JquerySelectorExtend selector, B @class)
        {
            return selector.Method("hasClass", Selector.Jquery.Class(@class).ToSelector().Replace(".", string.Empty));
        }

        /// <summary>
        ///     Get the current computed height for the first element in the set of matched elements
        /// </summary>
        public static JquerySelectorExtend Height(this JquerySelectorExtend selector)
        {
            return selector.Method("height");
        }

        /// <summary>
        ///     Get the current computed height for the first element in the set of matched elements, including padding but not border.
        /// </summary>
        public static JquerySelectorExtend InnerHeight(this JquerySelectorExtend selector)
        {
            return selector.Method("innerHeight");
        }

        /// <summary>
        ///     Get the current computed width for the first element in the set of matched elements, including padding but not border.
        /// </summary>
        public static JquerySelectorExtend InnerWidth(this JquerySelectorExtend selector)
        {
            return selector.Method("innerWidth");
        }

        /// <summary>
        ///     Check the current matched set of elements against a selector, element and return true
        /// </summary>
        public static JquerySelectorExtend Is(this JquerySelectorExtend selector, JquerySelector isSelector)
        {
            return selector.Method("is", isSelector);
        }

        /// <summary>
        ///     Check the current matched set of elements against a expression, element and return true
        /// </summary>
        public static JquerySelectorExtend Is(this JquerySelectorExtend selector, JqueryExpression expression)
        {
            return selector.Is(r => r.Expression(expression));
        }

        /// <summary>
        ///     Check the current matched set of elements against a tag, element and return true
        /// </summary>
        public static JquerySelectorExtend Is(this JquerySelectorExtend selector, HtmlTag tag)
        {
            return selector.Is(r => r.Tag(tag));
        }

        /// <summary>
        ///     Check the current matched set of elements against a selector, element and return true
        /// </summary>
        public static JquerySelectorExtend Is(this JquerySelectorExtend selector, Func<JquerySelector, JquerySelector> evaluated)
        {
            var isSelector = evaluated(Selector.Jquery);
            return selector.Is(isSelector);
        }


        /// <summary>
        ///     Get the current count element of matched elements
        /// </summary>
        public static JquerySelectorExtend Length(this JquerySelectorExtend selector)
        {            
            return selector.Property("length");
        }


        /// <summary>
        ///     Get the current value of the first element in the set of matched elements
        /// </summary>
        public static JquerySelectorExtend Val(this JquerySelectorExtend selector)
        {
            return selector.Method("val");
        }

        /// <summary>
        ///     Call method
        /// </summary>
        public static JquerySelectorExtend Method(this JquerySelectorExtend selector, string method, params object[] args)
        {
            var closureSelector = new JquerySelectorExtend(selector);
            closureSelector.AddMethod(method, args);
            return closureSelector;
        }

        /// <summary>
        ///     Get the current computed height for the first element in the set of matched elements,
        ///     including padding, border, and optionally margin. Returns an integer (without “px”) representation of the value or null if called on an empty set of elements.
        /// </summary>
        public static JquerySelectorExtend OuterHeight(this JquerySelectorExtend selector)
        {
            return selector.Method("outerHeight");
        }

        /// <summary>
        ///     Get the current computed width for the first element in the set of matched elements, including padding and border.
        /// </summary>
        public static JquerySelectorExtend OuterWidth(this JquerySelectorExtend selector)
        {
            return selector.Method("outerWidth");
        }

        /// <summary>
        ///     Get property
        /// </summary>
        public static JquerySelectorExtend Property(this JquerySelectorExtend selector, string prop)
        {
            var closureSelector = new JquerySelectorExtend(selector);
            closureSelector.AddProperty(prop);
            return closureSelector;
        }

        /// <summary>
        ///     Get the current horizontal position of the scroll bar for the first element in the set of matched elements the horizontal position of the scroll bar for every matched element.
        /// </summary>
        public static JquerySelectorExtend ScrollLeft(this JquerySelectorExtend selector)
        {
            return selector.Method("scrollLeft");
        }

        /// <summary>
        ///     Get the current horizontal position of the scroll bar for the first element in the set of matched elements the vertical position of the scroll bar for every matched element.
        /// </summary>
        public static JquerySelectorExtend ScrollTop(this JquerySelectorExtend selector)
        {
            return selector.Method("scrollTop");
        }

        /// <summary>
        ///     Get the combined text contents of each element in the set of matched elements, including their descendants
        /// </summary>
        public static JquerySelectorExtend Text(this JquerySelectorExtend selector)
        {
            return selector.Method("text");
        }

        /// <summary>
        ///     Get the current computed width for the first element in the set of matched elements  the width of every matched element.
        /// </summary>
        public static JquerySelectorExtend Width(this JquerySelectorExtend selector)
        {
            return selector.Method("width");
        }

        #endregion
    }
}