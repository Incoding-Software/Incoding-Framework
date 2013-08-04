namespace Incoding.MvcContrib
{
    public static class JquerySelectorExtendValueExtensions
    {
        #region Factory constructors

        /// <summary>
        ///     Get the determine whether any of the matched elements are assigned the given class.
        /// </summary>
        public static JquerySelectorExtend HasClass(this JquerySelectorExtend selector, string @class)
        {
            selector.AddMethod("hasClass", Selector.Jquery.Class(@class).ToSelector());
            return selector;
        }

        /// <summary>
        ///     Get the current computed height for the first element in the set of matched elements
        /// </summary>
        public static JquerySelectorExtend Height(this JquerySelectorExtend selector)
        {
            selector.AddMethod("height");
            return selector;
        }

        /// <summary>
        ///     Get the current computed height for the first element in the set of matched elements, including padding but not border.
        /// </summary>
        public static JquerySelectorExtend InnerHeight(this JquerySelectorExtend selector)
        {
            selector.AddMethod("innerHeight");
            return selector;
        }

        /// <summary>
        ///     Get the current computed width for the first element in the set of matched elements, including padding but not border.
        /// </summary>
        public static JquerySelectorExtend InnerWidth(this JquerySelectorExtend selector)
        {
            selector.AddMethod("innerWidth");
            return selector;
        }

        /// <summary>
        ///     Get the current count element of matched elements
        /// </summary>
        public static JquerySelectorExtend Length(this JquerySelectorExtend selector)
        {
            selector.AddProperty("length");
            return selector;
        }

        /// <summary>
        ///     Get the current computed height for the first element in the set of matched elements,
        ///     including padding, border, and optionally margin. Returns an integer (without “px”) representation of the value or null if called on an empty set of elements.
        /// </summary>
        public static JquerySelectorExtend OuterHeight(this JquerySelectorExtend selector)
        {
            selector.AddMethod("outerHeight");
            return selector;
        }

        /// <summary>
        ///     Get the current computed width for the first element in the set of matched elements, including padding and border.
        /// </summary>
        public static JquerySelectorExtend OuterWidth(this JquerySelectorExtend selector)
        {
            selector.AddMethod("outerWidth");
            return selector;
        }

        /// <summary>
        ///     Get the current horizontal position of the scroll bar for the first element in the set of matched elements the horizontal position of the scroll bar for every matched element.
        /// </summary>
        public static JquerySelectorExtend ScrollLeft(this JquerySelectorExtend selector)
        {
            selector.AddMethod("scrollLeft");
            return selector;
        }

        /// <summary>
        ///     Get the current horizontal position of the scroll bar for the first element in the set of matched elements the vertical position of the scroll bar for every matched element.
        /// </summary>
        public static JquerySelectorExtend ScrollTop(this JquerySelectorExtend selector)
        {
            selector.AddMethod("scrollTop");
            return selector;
        }

        /// <summary>
        ///     Get the combined text contents of each element in the set of matched elements, including their descendants
        /// </summary>
        public static JquerySelectorExtend Text(this JquerySelectorExtend selector)
        {
            selector.AddMethod("text");
            return selector;
        }

        /// <summary>
        ///     Get the current computed width for the first element in the set of matched elements  the width of every matched element.
        /// </summary>
        public static JquerySelectorExtend Width(this JquerySelectorExtend selector)
        {
            selector.AddMethod("width");
            return selector;
        }

        #endregion
    }
}