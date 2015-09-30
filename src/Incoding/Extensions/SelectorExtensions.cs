namespace Incoding.Extensions
{
    #region << Using >>

    using System;
    using Incoding.MvcContrib;
    using Incoding.MvcContrib.MVD;

    #endregion

    public static class SelectorExtensions
    {
        #region Factory constructors

        public static Selector ToAjax(this string url, Action<JqueryAjaxOptions> configuration)
        {
            return Selector.Incoding.Ajax(options =>
                                          {
                                              options.Url = url;
                                              configuration(options);
                                          });
        }

        public static Selector ToAjax(this UrlDispatcher.UrlPush url, Action<JqueryAjaxOptions> configuration)
        {
            return url.ToString().ToAjax(configuration);
        }

        public static Selector ToAjaxGet(this string url)
        {
            return Selector.Incoding.AjaxGet(url);
        }

        public static Selector ToAjaxGet(this UrlDispatcher.UrlPush url)
        {
            return url.ToString().ToAjaxGet();
        }

        public static Selector ToAjaxPost(this UrlDispatcher.UrlPush url)
        {
            return url.ToString().ToAjaxPost();
        }

        public static Selector ToAjaxPost(this string url)
        {
            return Selector.Incoding.AjaxPost(url);
        }

        public static Selector ToBuildUrl(this string url)
        {
            return Selector.Incoding.BuildUrl(url);
        }

        public static JquerySelectorExtend ToClass(this string @class)
        {
            return Selector.Jquery.Class(@class);
        }

        public static Selector ToConfirm(this string message)
        {
            return Selector.JS.Confirm(message);
        }

        public static JquerySelectorExtend ToId(this string id)
        {
            return Selector.Jquery.Id(id);
        }

        public static JquerySelectorExtend ToName(this string name)
        {
            return Selector.Jquery.Name(name);
        }

        #endregion
    }
}