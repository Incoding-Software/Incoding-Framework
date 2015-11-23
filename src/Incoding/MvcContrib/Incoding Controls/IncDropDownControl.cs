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

        JqueryAjaxOptions options = new JqueryAjaxOptions(IncodingHtmlHelper.DropDownOption);

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

        public JqueryAjaxOptions Options { get { return options; } set { options = value; } }

        public string Url { get { return Options.Url; } set { Options.Url = value; } }

        public IncSelectList Data { get; set; }

        public Selector Template { get; set; }

        public IEnumerable<KeyValueVm> Optional { get; set; }

        #endregion

        #region Nested classes

        public class IncSelectList
        {
            #region Fields

            readonly IList<KeyValueVm> keyValueVms;

            #endregion

            #region Constructors

            public IncSelectList(IEnumerable<KeyValueVm> keyValueVms)
            {
                this.keyValueVms = keyValueVms.ToList();
            }

            public IncSelectList(SelectList asSelectList)
            {
                keyValueVms = new List<KeyValueVm>();
                foreach (var item in asSelectList.Items)
                {
                    keyValueVms.Add(item.GetType().IsTypicalType()
                                            ? new KeyValueVm(item.ToString())
                                            : new KeyValueVm(item.TryGetValue(asSelectList.DataValueField), item.TryGetValue(asSelectList.DataTextField).With(r => r.ToString())));
                }
            }

            #endregion

            #region Properties

            public SelectList AsSelectList { get { return new SelectList(keyValueVms, "Value", "Text"); } }

            #endregion

            #region Api Methods

            public void AddOptional(IList<KeyValueVm> defaults)
            {
                for (int i = 0; i < defaults.Count; i++)
                    keyValueVms.Insert(i, defaults[i]);
            }

            #endregion

            public static implicit operator IncSelectList(List<KeyValueVm> s)
            {
                return new IncSelectList(s);
            }

            public static implicit operator IncSelectList(KeyValueVm[] s)
            {
                return new IncSelectList(s);
            }

            public static implicit operator IncSelectList(OptGroupVm s)
            {
                return new IncSelectList(s.Items);
            }

            public static implicit operator IncSelectList(SelectList s)
            {
                return new IncSelectList(s);
            }
        }

        #endregion

        public override MvcHtmlString ToHtmlString()
        {
            bool isAjax = !string.IsNullOrWhiteSpace(Url);

            bool isIml = OnInit != null ||
                         OnChange != null ||
                         OnEvent != null;

            if (isAjax || isIml)
            {
                var meta = isAjax ? htmlHelper.When(InitBind).Do().Ajax(options =>
                                                                        {
                                                                            options.Url = Url;
                                                                            options.Type = HttpVerbs.Get;
                                                                        })
                                   : htmlHelper.When(InitBind).Do().Direct();
                attributes = meta.OnSuccess(dsl =>
                                            {
                                                if (isAjax)
                                                {
                                                    dsl.Self().Insert.WithTemplate(Template).Html();
                                                    foreach (var vm in Optional.Recovery(new KeyValueVm[0]))
                                                    {
                                                        var option = new TagBuilder(HtmlTag.Option.ToStringLower());
                                                        option.SetInnerText(vm.Text);
                                                        option.MergeAttribute(HtmlAttribute.Value.ToStringLower(), vm.Value);
                                                        dsl.Self().JQuery.Dom.Use(new MvcHtmlString(option.ToString()).ToHtmlString()).Prepend();
                                                    }

                                                    var selected = ModelMetadata.FromLambdaExpression(property, htmlHelper.ViewData).Model;
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
                                 .AsHtmlAttributes(attributes);
            }

            if (!isAjax)
                Data.AddOptional(Optional.Recovery(new KeyValueVm[0]).ToList());

            return htmlHelper.DropDownListFor(property, Data.AsSelectList, attributes);
        }
    }
}