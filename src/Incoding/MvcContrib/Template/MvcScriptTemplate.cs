namespace Incoding.MvcContrib
{
    #region << Using >>

    using System.Web.Mvc;
    using Incoding.Extensions;

    #endregion

    public class MvcScriptTemplate<TModel> : MvcTemplate<TModel>
    {
        #region Fields

        readonly HtmlHelper htmlHelper;

        #endregion

        #region Constructors

        public MvcScriptTemplate(HtmlHelper htmlHelper, string id, string htmlType)
                : base(htmlHelper)
        {
            this.htmlHelper = htmlHelper;
            this.htmlHelper.ViewContext.Writer.Write("<script id=\"{0}\" type=\"{1}\">".F(id, htmlType));
        }

        #endregion

        public override void Dispose()
        {
            this.htmlHelper.ViewContext.Writer.Write("</script>");
        }
    }
}