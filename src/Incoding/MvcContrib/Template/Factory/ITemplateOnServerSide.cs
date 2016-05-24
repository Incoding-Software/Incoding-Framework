namespace Incoding.MvcContrib
{
    #region << Using >>

    using System.Web.Mvc;

    #endregion

    public interface ITemplateOnServerSide
    {

        string Render<T>(HtmlHelper htmlHelper, string pathToView, T data, object modelForView = null) where T : class;
    }
}