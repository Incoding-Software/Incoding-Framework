namespace Incoding.MvcContrib
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Web.Mvc;
    using System.Web.Mvc.Html;
    using Incoding.Extensions;
    using Incoding.Maybe;

    #endregion

    public class IncDropDownControl<TModel, TProperty> : IncControlBase
    {
        #region Constructors

        public IncDropDownControl(HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> property)
        {
            this.htmlHelper = htmlHelper;
            this.property = property;
            Data = new SelectList(new string[0]);
            Template = IncodingHtmlHelper.DropDownTemplateId;
            InitBind = JqueryBind.InitIncoding;            
        }

        #endregion

        public override MvcHtmlString ToHtmlString()
        {
            string currentUrl = Data;
            bool isAjax = !string.IsNullOrWhiteSpace(currentUrl);

            var meta = isAjax ? this.htmlHelper.When(InitBind).Ajax(currentUrl)
                               : this.htmlHelper.When(InitBind);
            attributes = meta.OnSuccess(dsl =>
                                        {
                                            if (isAjax)
                                            {
                                                dsl.Self().JQuery.Dom.Empty();
                                                foreach (var vm in (List<KeyValueVm>)Data.Optional)
                                                {
                                                    var option = new TagBuilder(HtmlTag.Option.ToStringLower());
                                                    option.SetInnerText(vm.Text);
                                                    option.MergeAttribute(HtmlAttribute.Value.ToStringLower(), vm.Value);
                                                    dsl.Self().JQuery.Dom.Use(new MvcHtmlString(option.ToString()).ToHtmlString()).Prepend();
                                                }
                                                dsl.Self().Insert.WithTemplate(Template).Append();
                                            }

                                            var selected = ModelMetadata.FromLambdaExpression(property, htmlHelper.ViewData).Model;
                                            if (selected != null)
                                                dsl.Self().JQuery.Attr.Val(selected);

                                            OnInit.Do(action => action(dsl));
                                            OnEvent.Do(action => action(dsl));
                                        })
                             .When(JqueryBind.Change)
                             .OnSuccess(dsl =>
                                        {
                                            OnChange.Do(action => action(dsl));
                                            OnEvent.Do(action => action(dsl));
                                        })
                             .AsHtmlAttributes(this.attributes);

            return this.htmlHelper.DropDownListFor(this.property, isAjax ? new SelectList(new string[] { }) : (SelectList)Data, this.attributes);
        }

        #region Fields

        readonly HtmlHelper<TModel> htmlHelper;

        readonly Expression<Func<TModel, TProperty>> property;

        JqueryAjaxOptions options = new JqueryAjaxOptions(IncodingHtmlHelper.DropDownOption);

        #endregion

        #region Properties

        public JqueryBind InitBind { get; set; }

        public JqueryAjaxOptions Options { get { return this.options; } set { this.options = value; } }

        public IncSelectList Data { get; set; }

        public Selector Template { get; set; }

        #endregion
    }
}