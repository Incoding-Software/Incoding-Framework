namespace Incoding.MvcContrib
{
    #region << Using >>

    using System;
    using System.Linq.Expressions;
    using System.Web.Mvc;
    using System.Web.Mvc.Html;
    using Incoding.Extensions;

    #endregion

    public class IncDropDownControl<TModel, TProperty> : IncControlBase
    {
        #region Fields

        readonly HtmlHelper<TModel> htmlHelper;

        readonly Expression<Func<TModel, TProperty>> property;

        #endregion

        #region Constructors

        public IncDropDownControl(HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> property)
        {
            this.htmlHelper = htmlHelper;
            this.property = property;
            Data = new SelectList(new string[0]);
            Template = Selector.Jquery.Id(IncodingHtmlHelper.DropDownTemplateId);
        }

        #endregion

        #region Properties

        public string Url { get; set; }

        public SelectList Data { get; set; }

        public JquerySelector Dependency { get; set; }

        public Selector Template { get; set; }

        public string Optional { get; set; }

        #endregion

        public override MvcHtmlString Render()
        {
            if (!string.IsNullOrWhiteSpace(Url))
            {
                var meta = new IncodingMetaLanguageDsl(JqueryBind.InitIncoding)
                        .Do().AjaxGet(Url)
                        .OnSuccess(dsl =>
                                       {
                                           dsl.Self().Core().Insert.WithTemplate(Template).Html();

                                           if (Optional != null)
                                           {
                                               var option = new TagBuilder(HtmlTag.Option.ToStringLower());
                                               option.SetInnerText(Optional);
                                               option.MergeAttribute(HtmlAttribute.Value.ToStringLower(), "");
                                               dsl.Self().Core().JQuery.Manipulation.Prepend(new MvcHtmlString(option.ToString()).ToHtmlString());
                                           }

                                           var selected = ModelMetadata.FromLambdaExpression(this.property, this.htmlHelper.ViewData).Model;
                                           if (selected != null)
                                               dsl.Self().Core().JQuery.Attributes.Val(selected);

                                           dsl.Self().Core().Trigger.Invoke(JqueryBind.Change);
                                       });

                if (Dependency != null)
                {
                    meta
                            .When(JqueryBind.Change)
                            .Do().Direct()
                            .OnSuccess(dsl => dsl.With(Dependency).Core().Trigger.Incoding());
                }

                this.attributes.Merge(meta.AsHtmlAttributes());
            }

            return Optional != null
                           ? this.htmlHelper.DropDownListFor(this.property, Data, Optional, this.attributes)
                           : this.htmlHelper.DropDownListFor(this.property, Data, this.attributes);
        }
    }
}