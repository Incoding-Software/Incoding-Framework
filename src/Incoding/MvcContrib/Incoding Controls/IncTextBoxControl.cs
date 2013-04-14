namespace Incoding.MvcContrib
{
    #region << Using >>

    using System;
    using System.Linq.Expressions;
    using System.Web.Mvc;
    using System.Web.Mvc.Html;
    using Incoding.Extensions;

    #endregion

    public class IncTextBoxControl<TModel, TProperty> : IncControlBase
    {
        #region Fields

        readonly HtmlHelper<TModel> htmlHelper;

        readonly Expression<Func<TModel, TProperty>> property;

        #endregion

        #region Constructors

        public IncTextBoxControl(HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> property)
        {
            this.htmlHelper = htmlHelper;
            this.property = property;
        }

        #endregion

        #region Properties

        /// <summary>
        ///     <see cref="HtmlAttribute.Placeholder" />
        /// </summary>
        public string Placeholder
        {
            set { this.attributes.Set(HtmlAttribute.Placeholder.ToStringLower(), value); }
        }

        #endregion

        public override MvcHtmlString Render()
        {
            return this.htmlHelper.TextBoxFor(this.property, this.attributes);
        }
    }
}