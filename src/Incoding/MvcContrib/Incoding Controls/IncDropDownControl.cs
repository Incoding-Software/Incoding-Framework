namespace Incoding.MvcContrib
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Web.Mvc;
    using System.Web.Mvc.Html;
    using Incoding.Extensions;
    using Incoding.Maybe;

    #endregion

    public class IncDropDownControl<TModel, TProperty> : IncControlBase
    {
        #region Fields

        readonly HtmlHelper<TModel> htmlHelper;

        readonly Expression<Func<TModel, TProperty>> property;

        private JqueryAjaxOptions options = new JqueryAjaxOptions(IncodingHtmlHelper.DropDownOption);

        #endregion

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

        #region Properties

        public JqueryBind InitBind { get; set; }

        public JqueryAjaxOptions Options { get { return this.options; } set { this.options = value; } }

        public string Url { get { return Options.Url; } set { Options.Url = value; } }

        public SelectList Data { get; set; }

        public Selector Template { get; set; }

        public IEnumerable<KeyValueVm> Optional { get; set; }

        #endregion

        public override MvcHtmlString ToHtmlString()
        {
            var optionals = Optional.Recovery(new KeyValueVm[0]);
            bool isAjax = !string.IsNullOrWhiteSpace(Url);

            bool isIml = OnInit != null ||
                         OnChange != null ||
                         this.OnEvent != null;

            if (isAjax || isIml)
            {
                var meta = isAjax ? this.htmlHelper.When(InitBind).Do().Ajax(options =>
                                                                             {
                                                                                 options.Url = Url;
                                                                                 options.Type = HttpVerbs.Get;
                                                                             })
                                   : this.htmlHelper.When(InitBind).Do().Direct();
                this.attributes.Merge(meta.OnSuccess(dsl =>
                                                     {
                                                         if (isAjax)
                                                         {
                                                             dsl.Self().Insert.WithTemplate(Template).Html();
                                                             foreach (var vm in optionals)
                                                             {
                                                                 var option = new TagBuilder(HtmlTag.Option.ToStringLower());
                                                                 option.SetInnerText(vm.Text);
                                                                 option.MergeAttribute(HtmlAttribute.Value.ToStringLower(), vm.Value);
                                                                 dsl.Self().JQuery.Dom.Use(new MvcHtmlString(option.ToString()).ToHtmlString()).Prepend();
                                                             }

                                                             var selected = ModelMetadata.FromLambdaExpression(this.property, this.htmlHelper.ViewData).Model;
                                                             if (selected != null)
                                                                 dsl.Self().JQuery.Attributes.Val(selected);
                                                         }

                                                         OnInit.Do(action => action(dsl));
                                                         OnEvent.Do(action => action(dsl));
                                                     })
                                          .When(JqueryBind.Change)
                                          .OnSuccess(dsl =>
                                                     {
                                                         OnChange.Do(action => action(dsl));
                                                         OnEvent.Do(action => action(dsl));
                                                     })
                                          .AsHtmlAttributes());
            }

            return !isAjax && optionals.Any()
                           ? this.htmlHelper.DropDownListFor(this.property, Data, optionals.FirstOrDefault().Text, this.attributes)
                           : this.htmlHelper.DropDownListFor(this.property, Data, this.attributes);
        }
    }
}