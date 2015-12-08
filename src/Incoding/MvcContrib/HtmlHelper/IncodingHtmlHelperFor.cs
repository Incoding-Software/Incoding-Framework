namespace Incoding.MvcContrib
{
    #region << Using >>

    using System;
    using System.Linq.Expressions;
    using System.Web.Mvc;
    using Incoding.Maybe;

    #endregion

    public class IncodingHtmlHelperFor<TModel, TProperty>
    {
        #region Fields

        readonly HtmlHelper<TModel> htmlHelper;

        readonly Expression<Func<TModel, TProperty>> property;

        #endregion

        #region Constructors

        public IncodingHtmlHelperFor(HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> property)
        {
            this.htmlHelper = htmlHelper;
            this.property = property;
        }

        #endregion

        #region Api Methods

        public MvcHtmlString Password(Action<IncPasswordControl<TModel, TProperty>> configuration = null)
        {
            return Control(new IncPasswordControl<TModel, TProperty>(this.htmlHelper, this.property), configuration);
        }

        public MvcHtmlString TextArea(Action<IncTextAreaControl<TModel, TProperty>> configuration = null)
        {
            return Control(new IncTextAreaControl<TModel, TProperty>(this.htmlHelper, this.property), configuration);
        }

        public MvcHtmlString TextBox(Action<IncTextBoxControl<TModel, TProperty>> configuration = null)
        {
            return Control(new IncTextBoxControl<TModel, TProperty>(this.htmlHelper, this.property), configuration);
        }

        public MvcHtmlString File(Action<IncFileControl<TModel, TProperty>> configuration = null)
        {
            return Control(new IncFileControl<TModel, TProperty>(this.htmlHelper, this.property), configuration);
        }

        public MvcHtmlString DropDown(Action<IncDropDownControl<TModel, TProperty>> configuration)
        {
            return Control(new IncDropDownControl<TModel, TProperty>(this.htmlHelper, this.property), configuration);
        }

        public MvcHtmlString ListBox(Action<IncListBoxControl<TModel, TProperty>> configuration)
        {
            return Control(new IncListBoxControl<TModel, TProperty>(this.htmlHelper, this.property), configuration);
        }

        public MvcHtmlString CheckBox(Action<IncCheckBoxControl<TModel, TProperty>> configuration = null)
        {
            return Control(new IncCheckBoxControl<TModel, TProperty>(this.htmlHelper, this.property), configuration);
        }

        public MvcHtmlString RadioButton(Action<IncRadioButtonControl<TModel, TProperty>> configuration = null)
        {
            return Control(new IncRadioButtonControl<TModel, TProperty>(this.htmlHelper, this.property), configuration);
        }

        public MvcHtmlString Hidden(Action<IncHiddenControl<TModel, TProperty>> configuration = null)
        {
            return Control(new IncHiddenControl<TModel, TProperty>(this.htmlHelper, this.property), configuration);
        }

        #endregion

        MvcHtmlString Control<TInput>(TInput input, Action<TInput> configuration) where TInput : IncControlBase
        {
            configuration.Do(action => action(input));
            return input.ToHtmlString();
        }
    }
}