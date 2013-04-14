namespace Incoding.MvcContrib
{
    #region << Using >>

    using System;
    using System.Linq.Expressions;
    using System.Web.Mvc;
    using Incoding.Extensions;

    #endregion

    public class IncListBoxControl<TModel, TProperty> : IncDropDownControl<TModel, TProperty>
    {
        #region Constructors

        public IncListBoxControl(HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> property)
                : base(htmlHelper, property)
        {
            this.attributes.Set(HtmlAttribute.Multiple.ToStringLower(), "multiple");
        }

        #endregion
    }
}