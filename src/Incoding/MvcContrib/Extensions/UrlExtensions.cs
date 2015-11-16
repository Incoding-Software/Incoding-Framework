namespace Incoding.MvcContrib
{
    #region << Using >>

    using System;
    using System.Web;
    using System.Web.Mvc;
    using Incoding.Extensions;
    using Incoding.Maybe;
    using Incoding.MvcContrib.MVD;
    using JetBrains.Annotations;

    #endregion

    public static class UrlExtensions
    {
        #region Factory constructors

        public static string ActionArea(this UrlHelper urlHelper, [AspMvcAction] string action, [AspMvcController] string controller, [AspMvcArea] string area, object routes = null)
        {
            Guard.NotNullOrWhiteSpace("action", action);
            Guard.NotNullOrWhiteSpace("controller", controller);

            var routeValues = AnonymousHelper.ToDictionary(routes);
            routeValues.Set("area", area);
            return urlHelper.Action(action, controller, routeValues);
        }

        public static UrlDispatcher Dispatcher(this UrlHelper urlHelper)
        {
            return new UrlDispatcher(urlHelper);
        }

        public static string Hash(this UrlHelper urlHelper, [AspMvcAction] string action, [AspMvcController] string controller, object routes = null)
        {
            return urlHelper.InternalHash(action, controller, urlHelper.RequestContext.HttpContext.Request.Url, string.Empty, routes);
        }

        public static string HashArea(this UrlHelper urlHelper, [AspMvcAction] string action, [AspMvcController] string controller, [AspMvcArea] string area = "", object routes = null)
        {
            return urlHelper.InternalHash(action, controller, urlHelper.RequestContext.HttpContext.Request.Url, area, routes);
        }

        public static string HashReferral(this UrlHelper urlHelper, [AspMvcAction] string action, [AspMvcController] string controller, object routes = null)
        {
            return urlHelper.InternalHash(action, controller, urlHelper.RequestContext.HttpContext.Request.UrlReferrer, string.Empty, routes);
        }

        public static string HashReferralArea(this UrlHelper urlHelper, [AspMvcAction] string action, [AspMvcController] string controller, [AspMvcArea] string area = "", object routes = null)
        {
            return urlHelper.InternalHash(action, controller, urlHelper.RequestContext.HttpContext.Request.UrlReferrer, area, routes);
        }

        #endregion

        static string InternalHash(this UrlHelper urlHelper, [AspMvcAction] string action, [AspMvcController] string controller, Uri uri, [AspMvcArea] string area = "", object routes = null)
        {
            string baseUrl = "/";

            if (uri.If(r => r.Segments.Length > 1).Has())
            {
                baseUrl = baseUrl
                        .AppendSegment(uri.Segments[0])
                        .AppendSegment(uri.Segments[1].Replace("/", string.Empty));
            }

            string urlHash = area
                    .Not(string.IsNullOrWhiteSpace)
                    .ReturnOrDefault(r => urlHelper.ActionArea(action, controller, area), urlHelper.Action(action, controller))
                    .AppendToHashQueryString(routes,true);

            return HttpUtility.UrlDecode(baseUrl.SetHash(urlHash));
        }
    }
}