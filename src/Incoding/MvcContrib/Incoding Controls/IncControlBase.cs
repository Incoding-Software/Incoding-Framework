namespace Incoding.MvcContrib
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Incoding.Extensions;
    using Incoding.Maybe;
    using Newtonsoft.Json.Linq;

    #endregion

    public abstract class IncControlBase
    {
        #region Fields

        protected RouteValueDictionary attributes = new RouteValueDictionary();

        #endregion

        protected RouteValueDictionary GetAttributes()
        {
            bool isIml = OnInit != null ||
                         OnChange != null ||
                         OnEvent != null;

            if (isIml)
            {
                attributes.Merge(new IncodingMetaLanguageDsl(JqueryBind.InitIncoding)
                                         .OnSuccess(dsl =>
                                                    {
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

            return attributes;
        }

        #region Properties

        /// <summary>
        ///     <see cref="HtmlAttribute.TabIndex" />
        /// </summary>
        public int TabIndex { set { attributes.Set(HtmlAttribute.TabIndex.ToStringLower(), value); } }

        public string Title { get { return this.attributes.GetOrDefault(HtmlAttribute.Title.ToStringLower(), string.Empty).ToString(); } set { attributes.Set(HtmlAttribute.Title.ToStringLower(), value); } }

        public bool Disabled
        {
            get { return this.attributes.ContainsKey(HtmlAttribute.Disabled.ToStringLower()); }
            set
            {
                string key = HtmlAttribute.Disabled.ToStringLower();
                if (value)
                    attributes.Set(key, key);
                else
                    attributes.Remove(key);
            }
        }

        public bool ReadOnly
        {
            get { return this.attributes.ContainsKey(HtmlAttribute.Readonly.ToStringLower()); }
            set
            {
                string key = HtmlAttribute.Readonly.ToStringLower();
                if (value)
                    attributes.Set(key, key);
                else
                    attributes.Remove(key);
            }
        }

        public Action<IIncodingMetaLanguageCallbackBodyDsl> OnEvent { get; set; }

        public Action<IIncodingMetaLanguageCallbackBodyDsl> OnInit { get; set; }

        public Action<IIncodingMetaLanguageCallbackBodyDsl> OnChange { get; set; }

        #endregion

        #region Api Methods

        public abstract MvcHtmlString ToHtmlString();

        public string GetAttr(HtmlAttribute attr)
        {
            return attributes[attr.ToStringLower()].With(r => r.ToString());
        }

        /// <summary>
        ///     <see cref="HtmlAttribute.AutoComplete" />
        /// </summary>
        public void SetAttr(HtmlAttribute attr, object value)
        {
            SetAttr(attr.ToStringLower(), value.With(r => r.ToString()));
        }

        /// <summary>
        ///     <see cref="HtmlAttribute" />
        /// </summary>
        public void SetAttr(HtmlAttribute attr)
        {
            SetAttr(attr.ToStringLower(), attr.ToJqueryString());
        }

        /// <summary>
        ///     <see cref="HtmlAttribute.AutoComplete" />
        /// </summary>
        public void RemoveAttr(HtmlAttribute attr)
        {
            RemoveAttr(attr.ToStringLower());
        }

        /// <summary>
        ///     <see cref="HtmlAttribute.AutoComplete" />
        /// </summary>
        public void RemoveAttr(string attr)
        {
            if (attributes.ContainsKey(attr))
                attributes.Remove(attr);
        }

        /// <summary>
        ///     <see cref="HtmlAttribute.AutoComplete" />
        /// </summary>
        public void SetAttr(string attr, object value)
        {
            attributes.Set(attr.ToLower(), value.With(r => r.ToString()));
        }

        public void Attr(RouteValueDictionary attr)
        {
            const string dataIncodingKey = "incoding";

            if (attr.ContainsKey(dataIncodingKey))
            {
                var meta = new List<object>();
                if (attributes.ContainsKey(dataIncodingKey))
                {
                    meta = (attributes[dataIncodingKey].ToString().DeserializeFromJson<object>() as JContainer)
                            .Cast<object>()
                            .ToList();
                }

                var newMeta = (attr[dataIncodingKey].ToString().DeserializeFromJson<object>() as JContainer).Cast<object>().ToList();
                meta.AddRange(newMeta);

                attr.Set(dataIncodingKey, meta.ToJsonString());
            }

            attributes.Merge(attr);
        }

        public void Attr(object attr)
        {
            Attr(AnonymousHelper.ToDictionary(attr));
        }

        public void AddClass(B @class)
        {
            AddClass(@class.AsClass());
        }

        public void AddClass(string @class)
        {
            const string key = "class";
            if (attributes.ContainsKey(key))
            {
                var orig = attributes[key].ToString();
                if (@class.Contains("col-xs-"))
                {
                    for (int i = 1; i <= 12; i++)
                        orig = orig.Replace("col-xs-{0}".F(i), "");
                }
                attributes[key] = orig + " " + @class;
            }
            else
                attributes.Add(key, @class);
        }

        #endregion
    }
}