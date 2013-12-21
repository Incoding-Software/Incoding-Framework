namespace Incoding.MvcContrib
{
    #region << Using >>

    using System.Linq.Expressions;
    using System.Web.Mvc;
    using System.Web.Mvc.Html;
    using Incoding.Extensions;

    #endregion

    public class IncValidationControl : IncControlBase
    {
        #region Fields

        readonly HtmlHelper htmlHelper;

        readonly string property;

        #endregion

        #region Constructors

        public IncValidationControl(HtmlHelper htmlHelper, LambdaExpression property)
        {
            this.htmlHelper = htmlHelper;
            this.property = property.GetMemberName();
        }

        #endregion

        public override MvcHtmlString ToHtmlString()
        {
            return this.htmlHelper.ValidationMessage(this.property, this.attributes);
        }
    }
}