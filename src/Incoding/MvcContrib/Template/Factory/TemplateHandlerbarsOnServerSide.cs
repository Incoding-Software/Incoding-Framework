namespace Incoding.MvcContrib
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using System.Web.Mvc.Html;
    using HandlebarsDotNet;
    using Incoding.Extensions;
    using JetBrains.Annotations;

    #endregion

    public class TemplateHandlebarsOnServerSide : ITemplateOnServerSide
    {
       internal static readonly Dictionary<string, Func<object, string>> cache = new Dictionary<string, Func<object, string>>();

        private readonly HtmlHelper htmlHelper;

        public TemplateHandlebarsOnServerSide(HtmlHelper htmlHelper)
        {
            this.htmlHelper = htmlHelper;
        }

        public string Render<T>([PathReference] string pathToView, T data)
        {
            return Render(pathToView, null, data);
        }

        public string Render<T>(string pathToView, object modelForView, T data)
        {
            var fullPathToView = pathToView.AppendToQueryString(modelForView);
            var compile = cache.GetOrAdd(fullPathToView, () => Handlebars.Compile(this.htmlHelper.Partial(pathToView, modelForView).ToHtmlString()));
            return compile(data);
        }
    }
}