namespace Incoding.MvcContrib
{
    #region << Using >>

    using System;
    using System.Web.Mvc;

    #endregion

    public class TemplateDoTFactory : ITemplateFactory
    {
        #region ITemplateFactory Members

        public ITemplateSyntax<TModel> ForEach<TModel>(HtmlHelper htmlHelper)
        {
            return new TemplateDoTSyntax<TModel>(htmlHelper, "data");
        }

        public ITemplateSyntax<TModel> NotEach<TModel>(HtmlHelper htmlHelper)
        {
            return new TemplateDoTSyntax<TModel>(htmlHelper, "data");
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
            throw new NotImplementedException();
        }

        #endregion
    }
}