namespace Incoding.MvcContrib
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Web.Mvc;
    using System.Web.Mvc.Html;
    using Incoding.Block.Logging;
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
            Optional = new IncSelectOptional();
        }

        #endregion

        public override MvcHtmlString ToHtmlString()
        {
            var currentUrl = Url ?? Data.Url;
            bool isAjax = !string.IsNullOrWhiteSpace(currentUrl);

            var meta = isAjax ? this.htmlHelper.When(InitBind).Ajax(currentUrl)
                               : this.htmlHelper.When(InitBind);
            attributes = meta.OnSuccess(dsl =>
                                        {
                                            if (isAjax)
                                            {
                                                dsl.Self().JQuery.Dom.Empty();
                                                foreach (var vm in Optional.Values)
                                                {
                                                    var option = new TagBuilder(HtmlTag.Option.ToStringLower());
                                                    option.SetInnerText(vm.Text);
                                                    option.MergeAttribute(HtmlAttribute.Value.ToStringLower(), vm.Value);
                                                    dsl.Self().JQuery.Dom.Use(new MvcHtmlString(option.ToString()).ToHtmlString()).Prepend();
                                                }
                                                dsl.Self().Insert.WithTemplate(Template).Append();
                                            }

                                            var selected = ModelMetadata.FromLambdaExpression(property, htmlHelper.ViewData).Model;
                                            var isMultiple = this.attributes.ContainsKey(HtmlAttribute.Multiple.ToString());
                                            if (isMultiple)
                                                dsl.Self().JQuery.Attr.Val(selected);
                                            else
                                            {
                                                if (selected != null)
                                                {
                                                    dsl.Self().JQuery.Attr.Val(selected)
                                                       .If(() => Selector.Jquery.Self().Find(HtmlTag.Option).Filter(r => r.EqualsAttribute(HtmlAttribute.Value, selected.ToString())).Length() > 0);
                                                }
                                                dsl.Self().JQuery.Attr.Val(Selector.Jquery.Self().Find(r => r.Tag(HtmlTag.Option).Expression(JqueryExpression.First)).Attr(HtmlAttribute.Value))
                                                   .If(() => Selector.Jquery.Self().Find(s => s.Tag(HtmlTag.Option).Expression(JqueryExpression.Selected)).Length() == 0);
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
                             .AsHtmlAttributes(this.attributes);

            if (!isAjax)
                Data.AddOptional(Optional.Values);

            return htmlHelper.DropDownListFor(this.property, isAjax ? new SelectList(new string[] { }) : Data.AsSelectList, this.attributes);
        }

        #region Fields

        readonly HtmlHelper<TModel> htmlHelper;

        readonly Expression<Func<TModel, TProperty>> property;

        JqueryAjaxOptions options = new JqueryAjaxOptions(IncodingHtmlHelper.DropDownOption);

        #endregion

        #region Properties

        public JqueryBind InitBind { get; set; }

        public JqueryAjaxOptions Options { get { return this.options; } set { this.options = value; } }

        [Obsolete("Please use Data instead of Url")]
        public string Url { get; set; }

        public IncSelectList Data { get; set; }

        public Selector Template { get; set; }

        public IncSelectOptional Optional { get; set; }

        #endregion

        #region Nested classes

        public class IncSelectOptional
        {
            private readonly List<KeyValueVm> values = new List<KeyValueVm>();

            public IncSelectOptional(string value)
                    : this(new KeyValueVm(value)) { }

            public IncSelectOptional(KeyValueVm value)
                    : this(new[] { value }) { }

            public IncSelectOptional(IEnumerable<KeyValueVm> value)
            {
                this.values = value.ToList();
            }

            public IncSelectOptional() { }

            public List<KeyValueVm> Values { get { return this.values; } }

            public static implicit operator IncSelectOptional(List<KeyValueVm> s)
            {
                return new IncSelectOptional(s);
            }

            public static implicit operator IncSelectOptional(KeyValueVm[] s)
            {
                return new IncSelectOptional(s);
            }

            public static implicit operator IncSelectOptional(string s)
            {
                return new IncSelectOptional(s);
            }

            public static implicit operator IncSelectOptional(string[] s)
            {
                return new IncSelectOptional(s.Select(r => new KeyValueVm(r)));
            }

            public static implicit operator IncSelectOptional(KeyValueVm s)
            {
                return new IncSelectOptional(s);
            }
        }

        public class IncSelectList
        {
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

            public static implicit operator IncSelectList(string url)
            {
                return new IncSelectList(url);
            }

            #region Fields

            readonly IList<KeyValueVm> keyValueVms = new List<KeyValueVm>();

            private readonly string url;

            #endregion

            #region Properties

            public SelectList AsSelectList { get { return new SelectList(keyValueVms, "Value", "Text"); } }

            public string Url { get { return this.url; } }

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

            private IncSelectList(string asSelectList)
            {
                this.url = asSelectList;
            }

            #endregion
        }

        #endregion
    }
}