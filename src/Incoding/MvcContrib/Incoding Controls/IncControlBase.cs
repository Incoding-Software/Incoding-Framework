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
                this.attributes.Merge(new IncodingMetaLanguageDsl(JqueryBind.InitIncoding)
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

            return this.attributes;
        }

        #region Properties

        /// <summary>
        ///     <see cref="HtmlAttribute.TabIndex" />
        /// </summary>
        public int TabIndex { set { this.attributes.Set(HtmlAttribute.TabIndex.ToStringLower(), value); } }

        public bool ReadOnly
        {
            set
            {
                string key = HtmlAttribute.Readonly.ToStringLower();
                if (value)
                    this.attributes.Set(key, key);
                else
                    this.attributes.Remove(key);
            }
        }

        public Action<IIncodingMetaLanguageCallbackBodyDsl> OnEvent { get; set; }

        public Action<IIncodingMetaLanguageCallbackBodyDsl> OnInit { get; set; }

        public Action<IIncodingMetaLanguageCallbackBodyDsl> OnChange { get; set; }

        #endregion

        #region Api Methods

        public abstract MvcHtmlString ToHtmlString();

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
            RemoveAttr(attr);
        }

        /// <summary>
        ///     <see cref="HtmlAttribute.AutoComplete" />
        /// </summary>
        public void RemoveAttr(string attr)
        {
            if (this.attributes.ContainsKey(attr))
                this.attributes.Remove(attr);
        }

        /// <summary>
        ///     <see cref="HtmlAttribute.AutoComplete" />
        /// </summary>
        public void SetAttr(string attr, object value)
        {
            this.attributes.Set(attr.ToLower(), value.With(r => r.ToString()));
        }

        public void Attr(RouteValueDictionary attr)
        {
            const string dataIncodingKey = "incoding";

            if (attr.ContainsKey(dataIncodingKey))
            {
                var meta = new List<object>();
                if (this.attributes.ContainsKey(dataIncodingKey))
                {
                    meta = (this.attributes[dataIncodingKey].ToString().DeserializeFromJson<object>() as JContainer)
                            .Cast<object>()
                            .ToList();
                }

                var newMeta = (attr[dataIncodingKey].ToString().DeserializeFromJson<object>() as JContainer).Cast<object>().ToList();
                meta.AddRange(newMeta);

                attr.Set(dataIncodingKey, meta.ToJsonString());
            }

            this.attributes.Merge(attr);
        }

        public void Attr(object attr)
        {
            Attr(AnonymousHelper.ToDictionary(attr));
        }

        public void AddClass(string @class)
        {
            const string key = "class";
            if (this.attributes.ContainsKey(key))
                this.attributes[key] += " " + @class;
            else
                this.attributes.Add(key, @class);
        }

        #endregion
    }
}