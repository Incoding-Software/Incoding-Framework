namespace Incoding.MvcContrib
{
    #region << Using >>

    using System;
    using System.Linq.Expressions;
    using System.Web.Mvc;
    using System.Web.Mvc.Html;
    using Incoding.Extensions;

    #endregion

    public class IncPasswordControl<TModel, TProperty> : IncControlBase
    {
        #region Fields

        readonly HtmlHelper<TModel> htmlHelper;

        readonly Expression<Func<TModel, TProperty>> property;

        #endregion

        public int MaxLength { set { this.attributes.Set(HtmlAttribute.MaxLength.ToStringLower(), value); } }

        public string Placeholder { set { this.attributes.Set(HtmlAttribute.Placeholder.ToStringLower(), value); } }

        #region Constructors

        public IncPasswordControl(HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> property)
        {
            this.htmlHelper = htmlHelper;
            this.property = property;
        }

        #endregion

        public override MvcHtmlString ToHtmlString()
        {
            return this.htmlHelper.PasswordFor(this.property, GetAttributes());
        }
    }
}