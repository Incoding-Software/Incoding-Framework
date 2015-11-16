namespace Incoding.MvcContrib
{
    #region << Using >>

    using System;
    using System.Linq.Expressions;
    using System.Web.Mvc;
    using Incoding.Extensions;

    #endregion

    // ReSharper disable IntroduceOptionalParameters.Global
    public class IncodingSelector : Selector
    {
        #region Constructors

        internal IncodingSelector(string selector)
                : base(selector) { }

        #endregion

        #region Api Methods

        public Selector HashQueryString(string key, string prefix)
        {
            AndSelector("||hashQueryString*{0}:{1}||".F(key, prefix));
            return this;
        }

        public Selector HashQueryString(string key)
        {
            return HashQueryString(key, "root");
        }

        public Selector HashQueryString<TModel>(Expression<Func<TModel, object>> property, string prefix)
        {
            return HashQueryString(property.GetMemberName(), prefix);
        }

        public Selector HashQueryString<TModel>(Expression<Func<TModel, object>> property)
        {
            return HashQueryString(property, "root");
        }

        public Selector HashUrl(string prefix)
        {
            AndSelector("||hashUrl*{0}||".F(prefix));
            return this;
        }

        public Selector HashUrl()
        {
            return HashUrl("root");
        }

        public Selector QueryString(string key)
        {
            AndSelector("||queryString*{0}||".F(key));
            return this;
        }

        public Selector QueryString<TModel>(Expression<Func<TModel, object>> property)
        {
            return QueryString(property.GetMemberName());
        }

        public Selector Cookie<TModel>(Expression<Func<TModel, object>> property)
        {
            return Cookie(property.GetMemberName());
        }

        public Selector Cookie(string key)
        {
            AndSelector("||cookie*{0}||".F(key));
            return this;
        }

        public Selector Ajax(Action<JqueryAjaxOptions> configuration)
        {
            var options = new JqueryAjaxOptions(JqueryAjaxOptions.Default);
            configuration(options);
            options.Async = false;
            AndSelector("||ajax*{0}||".F(options.ToJsonString()));
            return this;
        }

        public Selector AjaxGet(string url)
        {
            return Ajax(options =>
                        {
                            options.Url = url;
                            options.Type = HttpVerbs.Get;
                        });
        }

        public Selector AjaxPost(string url)
        {
            return Ajax(options =>
                        {
                            options.Url = url;
                            options.Type = HttpVerbs.Post;
                        });
        }

        public Selector BuildUrl(string url)
        {
            AndSelector("||buildurl*{0}||".F(url));
            return this;
        }

        #endregion
    }

    // ReSharper restore IntroduceOptionalParameters.Global
}