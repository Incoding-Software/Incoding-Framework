namespace Incoding.MvcContrib
{
    #region << Using >>

    using Incoding.Extensions;

    #endregion

    public class IncodingMetaCallbackJqueryCssDsl
    {
        #region Fields

        readonly IIncodingMetaLanguagePlugInDsl plugInDsl;

        #endregion

        #region Constructors

        public IncodingMetaCallbackJqueryCssDsl(IIncodingMetaLanguagePlugInDsl plugInDsl)
        {
            this.plugInDsl = plugInDsl;
        }

        #endregion

        #region Api Methods

        /// <summary>
        ///     Set one attributes for every matched element in context.
        /// </summary>
        /// <param name="key">Css attributes</param>
        /// <param name="value">
        ///     <see cref="Selector" />
        /// </param>
        public IExecutableSetting Set(string key, Selector value)
        {
            return this.plugInDsl.Core().JQuery.Call("css", key, value);
        }

        /// <summary>
        ///     Set one attributes for every matched element in context.
        /// </summary>
        /// <param name="key">
        ///     <see cref="CssStyling" />
        /// </param>
        /// <param name="value">
        ///     <see cref="Selector" />
        /// </param>
        public IExecutableSetting Set(CssStyling key, Selector value)
        {
            return Set(key.ToJqueryString(), value);
        }

        /// <summary>
        ///     Set css display attribute of every matched element.
        /// </summary>
        public IExecutableSetting Display(Display display)
        {
            return Set(CssStyling.Display, display.ToLocalization());
        }

        /// <summary>
        ///     Set the height of every matched element.
        /// </summary>
        /// <param name="value">
        ///     <see cref="Selector" />
        /// </param>
        public IExecutableSetting Height(Selector value)
        {
            return this.plugInDsl.Core().JQuery.Call("height", value);
        }

        /// <summary>
        ///     set the horizontal position of the scroll bar for every matched element.
        /// </summary>
        /// <param name="value">
        ///     <see cref="Selector" />
        /// </param>
        public IExecutableSetting ScrollLeft(Selector value)
        {
            return this.plugInDsl.Core().JQuery.Call("scrollLeft", value);
        }

        /// <summary>
        ///     set the vertical position of the scroll bar for every matched element.
        /// </summary>
        /// <param name="value">
        ///     <see cref="Selector" />
        /// </param>
        public IExecutableSetting ScrollTop(Selector value)
        {
            return this.plugInDsl.Core().JQuery.Call("scrollTop", value);
        }

        /// <summary>
        ///     set the width of every matched element.
        /// </summary>
        /// <param name="value">
        ///     <see cref="Selector" />
        /// </param>
        public IExecutableSetting Width(Selector value)
        {
            return this.plugInDsl.Core().JQuery.Call("width", value);
        }

        #endregion
    }
}