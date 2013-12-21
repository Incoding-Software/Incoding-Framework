namespace Incoding.MvcContrib
{
    using System.Web.Mvc;

    public interface ITemplateFactory
    {
        ITemplateSyntax<TModel> ForEach<TModel>(HtmlHelper htmlHelper);

        ITemplateSyntax<TModel> NotEach<TModel>(HtmlHelper htmlHelper);

        string GetDropDownTemplate();
    }
}