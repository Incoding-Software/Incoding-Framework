namespace Incoding.MvcContrib
{
    #region << Using >>

    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Routing;
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
            Guard.NotNullOrWhiteSpace("key", key);
            Guard.NotNull("value", value);

            return Set(new RouteValueDictionary(new Dictionary<string, object>
                                                    {
                                                            { key, value }
                                                    }));
        }

        public IExecutableSetting Set(object styles)
        {
            var codes = AnonymousHelper
                    .ToDictionary(styles)
                    .Select(r => JavaScriptCodeTemplate.Target_SetCss.F(r.Key.ToLower(), Selector.FromObject(r.Value)))
                    .ToList();

            return this.plugInDsl.Registry(new ExecutableEval(codes));
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
            return Set(key.ToStringLower().Replace("_", "-"), value);
        }

        /// <summary>
        ///     Set the height of every matched element.
        /// </summary>
        /// <param name="value">
        ///     <see cref="Selector" />
        /// </param>
        public IExecutableSetting Height(Selector value)
        {
            string code = JavaScriptCodeTemplate.Target_Height.F(value);
            return this.plugInDsl.Registry(new ExecutableEval(code));
        }

        /// <summary>
        ///     set the horizontal position of the scroll bar for every matched element.
        /// </summary>
        /// <param name="value">
        ///     <see cref="Selector" />
        /// </param>
        public IExecutableSetting ScrollLeft(Selector value)
        {
            string code = JavaScriptCodeTemplate.Target_ScrollLeft.F(value);
            return this.plugInDsl.Registry(new ExecutableEval(code));
        }

        /// <summary>
        ///     set the vertical position of the scroll bar for every matched element.
        /// </summary>
        /// <param name="value">
        ///     <see cref="Selector" />
        /// </param>
        public IExecutableSetting ScrollTop(Selector value)
        {
            string code = JavaScriptCodeTemplate.Target_ScrollTop.F(value);
            return this.plugInDsl.Registry(new ExecutableEval(code));
        }

        /// <summary>
        ///     set the width of every matched element.
        /// </summary>
        /// <param name="value">
        ///     <see cref="Selector" />
        /// </param>
        public IExecutableSetting Width(Selector value)
        {
            string code = JavaScriptCodeTemplate.Target_Width.F(value);
            return this.plugInDsl.Registry(new ExecutableEval(code));
        }

        #endregion
    }
}