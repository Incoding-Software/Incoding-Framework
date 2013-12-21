namespace Incoding.MvcContrib
{
    #region << Using >>

    using System.Web.Mvc;

    #endregion

    public class TemplateHandlebarsFactory : ITemplateFactory
    {
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

        #endregion
    }
}