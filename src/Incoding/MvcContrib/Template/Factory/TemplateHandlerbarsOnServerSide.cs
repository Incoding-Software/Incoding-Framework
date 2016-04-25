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

    #endregion

    public class TemplateHandlebarsOnServerSide : ITemplateOnServerSide
    {
        internal static readonly Dictionary<string, Func<object, string>> cache = new Dictionary<string, Func<object, string>>();

        public string Render<T>(HtmlHelper htmlHelper, string pathToView, T data, object modelForView = null)
        {
            var fullPathToView = pathToView.AppendToQueryString(modelForView);

            return cache.GetOrAdd(fullPathToView, () =>
                                                  {
                                                      var tmpl = IoCFactory.Instance.TryResolve<IDispatcher>().Query(new RenderViewQuery()
                                                                                                                     {
                                                                                                                             HtmlHelper = htmlHelper,
                                                                                                                             PathToView = pathToView,
                                                                                                                             Model = modelForView
                                                                                                                     }).ToHtmlString();
                                                      return Handlebars.Compile(tmpl);
                                                  })(new { data = data });
        }
    }
}