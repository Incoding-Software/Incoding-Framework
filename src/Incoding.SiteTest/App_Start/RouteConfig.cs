namespace Incoding.SiteTest.App_Start
{
    #region << Using >>

    using System.Web.Mvc;
    using System.Web.Routing;

    #endregion

    public class RouteConfig
    {
        #region Factory constructors

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{resource}.mvd/{*pathInfo}");
            routes.MapRoute(
                            name: "Default", 
                            url: "{controller}/{action}/{id}", 
                            namespaces: new[] { "Incoding.SiteTest.Controllers" }, 
                            defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional });
        }

        #endregion
    }
}