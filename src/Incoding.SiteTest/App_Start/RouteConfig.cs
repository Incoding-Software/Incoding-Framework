namespace Incoding.SiteTest.App_Start
{
    #region << Using >>

    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Incoding.MvcContrib.MVD;

    #endregion


    public class RouteConfig
    {
        #region Factory constructors

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{resource}.mvd/{*pathInfo}");

            RouteTable.Routes.Add(new Route("handler/query", QueryHttpHandler.Route));
            RouteTable.Routes.Add(new Route("handler/push", PushHttpHandler.Route));
            routes.MapRoute(
                            name: "Default",
                            url: "{controller}/{action}/{id}",
                            namespaces: new[] { "Incoding.SiteTest.Controllers" },
                            defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional });
        }

        #endregion
    }
}