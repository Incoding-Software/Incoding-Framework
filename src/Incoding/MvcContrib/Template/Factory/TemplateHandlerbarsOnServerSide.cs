namespace Incoding.MvcContrib
{
    #region << Using >>

    using System;
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Web.Mvc;
    using FluentNHibernate.Utils;
    using HandlebarsDotNet;
    using Incoding.Block.IoC;
    using Incoding.CQRS;
    using Incoding.Extensions;
    using Incoding.Maybe;

    #endregion

    public class TemplateHandlebarsOnServerSide : ITemplateOnServerSide
    {
        internal static readonly ConcurrentDictionary<string, Func<object, string>> cache = new ConcurrentDictionary<string, Func<object, string>>();

        public static string Version { get; set; }

        public string Render<T>(HtmlHelper htmlHelper, string pathToView, T data, object modelForView = null) where T : class
        {
            var fullPathToView = pathToView.AppendToQueryString(modelForView);

            object correctData = data;
            if (data != null && !data.GetType().HasInterface(typeof(IEnumerable)))
                correctData = new { data = data };

            return cache.GetOrAdd(fullPathToView + Version, (i) =>
                                                            {
                                                                var tmpl = IoCFactory.Instance.TryResolve<IDispatcher>().Query(new RenderViewQuery()
                                                                                                                               {
                                                                                                                                       HtmlHelper = htmlHelper,
                                                                                                                                       PathToView = pathToView,
                                                                                                                                       Model = modelForView
                                                                                                                               }).ToHtmlString();
                                                                return Handlebars.Compile(tmpl);
                                                            })(new { data = correctData });
        }
    }
}