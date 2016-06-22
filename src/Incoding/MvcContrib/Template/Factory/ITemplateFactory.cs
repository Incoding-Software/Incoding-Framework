namespace Incoding.MvcContrib
{
    using System.Web.Mvc;

    public interface ITemplateFactory
    {        
        ITemplateSyntax<TModel> ForEach<TModel>(HtmlHelper htmlHelper);

        ITemplateSyntax<TModel> NotEach<TModel>(HtmlHelper htmlHelper);

        string GetDropDownTemplate();

        string Render<T>(HtmlHelper htmlHelper, string pathToView, T data, object modelForView = null) where T : class;
    }
}