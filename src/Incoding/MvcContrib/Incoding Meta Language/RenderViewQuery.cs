namespace Incoding.MvcContrib
{
    #region << Using >>

    using System.Web.Mvc;
    using System.Web.Mvc.Html;
    using Incoding.CQRS;

    #endregion

    public class RenderViewQuery : QueryBase<MvcHtmlString>
    {
        public HtmlHelper HtmlHelper { get; set; }

        public string PathToView { get; set; }

        public object Model { get; set; }

        protected override MvcHtmlString ExecuteResult()
        {
            return this.HtmlHelper.Partial(PathToView, Model);
        }
    }
}