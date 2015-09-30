namespace Incoding.MvcContrib
{
    #region << Using >>

    using System;
    using System.Linq.Expressions;
    using System.Web.Mvc;
    using Incoding.Extensions;

    #endregion

    public class IncFileControl<TModel, TProperty> : IncControlBase
    {
        #region Fields

        readonly HtmlHelper<TModel> htmlHelper;

        #endregion

        #region Constructors

        public IncFileControl(HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> property)
        {
            this.htmlHelper = htmlHelper;
            this.attributes.Set("id", property.GetMemberNameAsHtmlId());
            this.attributes.Set("name", property.GetMemberName());
        }

        #endregion

        #region Properties

        public string Value { get; set; }

        #endregion

        public override MvcHtmlString ToHtmlString()
        {
            return this.htmlHelper.Incoding().File(Value, GetAttributes());
        }
    }
}