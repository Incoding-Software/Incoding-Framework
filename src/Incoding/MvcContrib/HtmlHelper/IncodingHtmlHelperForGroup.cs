namespace Incoding.MvcContrib
{
    #region << Using >>

    using System;
    using System.Linq.Expressions;
    using System.Web.Mvc;
    using Incoding.Maybe;

    #endregion

    public class IncodingHtmlHelperForGroup<TModel, TProperty>
    {
        #region Fields

        readonly HtmlHelper<TModel> htmlHelper;

        readonly Expression<Func<TModel, TProperty>> property;

        #endregion

        #region Constructors

        public IncodingHtmlHelperForGroup(HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> property)
        {
            this.htmlHelper = htmlHelper;
            this.property = property;
        }

        #endregion

        ////ncrunch: no coverage start
        #region Api Methods

        public MvcHtmlString Hidden(Action<IncHiddenControl<TModel, TProperty>> configuration = null)
        {
            var hidden = new IncHiddenControl<TModel, TProperty>(this.htmlHelper, this.property);
            configuration.Do(r => r(hidden));
            return hidden.ToHtmlString();
        }

        public MvcHtmlString CheckBox(Action<IncHorizontalControl<IncCheckBoxControl<TModel, TProperty>>> configuration = null)
        {
            return Group(new IncCheckBoxControl<TModel, TProperty>(this.htmlHelper, this.property), configuration);
        }

        public MvcHtmlString TextBox(Action<IncHorizontalControl<IncTextBoxControl<TModel, TProperty>>> configuration = null)
        {
            return Group(new IncTextBoxControl<TModel, TProperty>(this.htmlHelper, this.property), configuration);
        }

        public MvcHtmlString File(Action<IncHorizontalControl<IncFileControl<TModel, TProperty>>> configuration = null)
        {
            return Group(new IncFileControl<TModel, TProperty>(this.htmlHelper, this.property), configuration);
        }

        public MvcHtmlString TextArea(Action<IncHorizontalControl<IncTextAreaControl<TModel, TProperty>>> configuration = null)
        {
            return Group(new IncTextAreaControl<TModel, TProperty>(this.htmlHelper, this.property), configuration);
        }

        public MvcHtmlString Password(Action<IncHorizontalControl<IncPasswordControl<TModel, TProperty>>> configuration = null)
        {
            return Group(new IncPasswordControl<TModel, TProperty>(this.htmlHelper, this.property), configuration);
        }

        public MvcHtmlString DropDown(Action<IncHorizontalControl<IncDropDownControl<TModel, TProperty>>> configuration = null)
        {
            return Group(new IncDropDownControl<TModel, TProperty>(this.htmlHelper, this.property), configuration);
        }

        public MvcHtmlString ListBox(Action<IncHorizontalControl<IncListBoxControl<TModel, TProperty>>> configuration = null)
        {
            return Group(new IncListBoxControl<TModel, TProperty>(this.htmlHelper, this.property), configuration);
        }

        #endregion

        MvcHtmlString Group<TInput>(TInput input, Action<IncHorizontalControl<TInput>> configuration) where TInput : IncControlBase
        {
            var label = new IncLabelControl(this.htmlHelper, this.property);
            label.AddClass("control-label");
            var validation = new IncValidationControl(this.htmlHelper, this.property);
            var horizontal = new IncHorizontalControl<TInput>(label, input, validation);
            configuration.Do(r => r(horizontal));

            return horizontal.ToHtmlString();
        }

        ////ncrunch: no coverage end
    }
}