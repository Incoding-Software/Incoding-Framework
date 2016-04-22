namespace Incoding.MvcContrib
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using HandlebarsDotNet;
    using Incoding.Block.IoC;
    using Incoding.CQRS;
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

        public string Render<T>([PathReference] string pathToView, object modelForView, T data)
        {
            var fullPathToView = pathToView.AppendToQueryString(modelForView);

            return cache.GetOrAdd(fullPathToView, () =>
                                                  {
                                                      var tmpl = IoCFactory.Instance.TryResolve<IDispatcher>().Query(new RenderViewQuery()
                                                                                                                     {
                                                                                                                             HtmlHelper = this.htmlHelper,
                                                                                                                             PathToView = pathToView,
                                                                                                                             Model = modelForView
                                                                                                                     }).ToHtmlString();
                                                      return Handlebars.Compile(tmpl);
                                                  })(data);
        }
    }
}