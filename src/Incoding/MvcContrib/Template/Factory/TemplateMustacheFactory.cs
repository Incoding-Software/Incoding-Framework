namespace Incoding.MvcContrib
{
    #region << Using >>

    using System;
    using System.Web.Mvc;

    #endregion

    public class TemplateMustacheFactory : ITemplateFactory
    {
        #region ITemplateFactory Members

        public ITemplateSyntax<TModel> ForEach<TModel>(HtmlHelper htmlHelper)
        {
            return new TemplateMustacheSyntax<TModel>(htmlHelper, "data", true);
        }

        public ITemplateSyntax<TModel> NotEach<TModel>(HtmlHelper htmlHelper)
        {
            return new TemplateMustacheSyntax<TModel>(htmlHelper, "data", false);
        }

        public string GetDropDownTemplate()
        {
            return @"{{#data}}{{#Title}} <optgroup label=""{{Title}}"">
                               {{#Items}} 
                               <option {{#Selected}}selected=""selected""{{/Selected}} value=""{{Value}}"">{{Text}}</option>
                               {{/Items}} </optgroup>
                               {{/Title}}{{^Title}}
                               {{#Items}} <option {{#Selected}}selected=""selected""{{/Selected}} value=""{{Value}}"">{{Text}}</option>
                               {{/Items}}
                               {{/Title}}
                               {{/data}}";
        }

        public string Render<T>(HtmlHelper htmlHelper, string pathToView, T data, object modelForView = null) where T : class
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}