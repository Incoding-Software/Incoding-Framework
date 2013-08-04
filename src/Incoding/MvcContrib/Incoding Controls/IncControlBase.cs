namespace Incoding.MvcContrib
{
    #region << Using >>

    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Incoding.Extensions;
    using Newtonsoft.Json.Linq;

    #endregion

    public abstract class IncControlBase
    {
        #region Fields

        protected readonly RouteValueDictionary attributes = new RouteValueDictionary();

        #endregion

        #region Properties

        /// <summary>
        ///     <see cref="HtmlAttribute.TabIndex" />
        /// </summary>
        public int TabIndex { set { this.attributes.Set(HtmlAttribute.TabIndex.ToStringLower(), value); } }

        #endregion

        #region Api Methods

        public abstract MvcHtmlString Render();

        /// <summary>
        ///     <see cref="HtmlAttribute.AutoComplete" />
        /// </summary>
        public void DisableAutoComplete()
        {
            this.attributes.Set(HtmlAttribute.AutoComplete.ToStringLower(), "off");
        }

        public void Attr(object attr)
        {
            var allAttr = AnonymousHelper.ToDictionary(attr);

            const string dataIncodingKey = "incoding";

            if (allAttr.ContainsKey(dataIncodingKey))
            {
                var meta = new List<object>();
                if (this.attributes.ContainsKey(dataIncodingKey))
                {
                    meta = (this.attributes[dataIncodingKey].ToString().DeserializeFromJson<object>() as JContainer)
                            .Cast<object>()
                            .ToList();
                }

                var newMeta = (allAttr[dataIncodingKey].ToString().DeserializeFromJson<object>() as JContainer).Cast<object>().ToList();
                meta.AddRange(newMeta);

                allAttr.Set(dataIncodingKey, meta.ToJsonString());
            }

            this.attributes.Merge(allAttr);
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