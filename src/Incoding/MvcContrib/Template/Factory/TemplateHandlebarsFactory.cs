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

    #endregion

    public class TemplateHandlebarsFactory : ITemplateFactory
    {
        internal static readonly ConcurrentDictionary<string, Func<object, string>> cache = new ConcurrentDictionary<string, Func<object, string>>();

        public static Func<string> GetVersion = () => { return string.Empty; };

        #region ITemplateFactory Members

        public ITemplateSyntax<TModel> ForEach<TModel>(HtmlHelper htmlHelper)
        {
            return new TemplateHandlebarsSyntax<TModel>(htmlHelper, "data", HandlebarsType.Each, string.Empty);
        }

        public ITemplateSyntax<TModel> NotEach<TModel>(HtmlHelper htmlHelper)
        {
            return new TemplateHandlebarsSyntax<TModel>(htmlHelper, "data", HandlebarsType.Unless, string.Empty);
        }

        public string GetDropDownTemplate()
        {
            return @"{{#data}}
                                 {{#if Title}}
                                 <optgroup label=""{{Title}}"">
                                 {{#each Items}}
                                 <option {{#Selected}}selected=""selected""{{/Selected}} value=""{{Value}}"">{{Text}}</option>
                                 {{/each}}
                                 </optgroup>
                                 {{else}}
                                 {{#each Items}}
                                 <option {{#Selected}}selected=""selected""{{/Selected}} value=""{{Value}}"">{{Text}}</option>
                                 {{/each}}
                                 {{/if}}
                                 {{/data}}";
        }

        public string Render<T>(HtmlHelper htmlHelper, string pathToView, T data, object modelForView = null) where T : class
        {
            var fullPathToView = pathToView.AppendToQueryString(modelForView);
            object correctData = data;
            if (data != null && !data.GetType().HasInterface(typeof(IEnumerable)))
                correctData = new { data = data };

            return cache.GetOrAdd(fullPathToView + GetVersion(), (i) =>
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

        #endregion
    }
}