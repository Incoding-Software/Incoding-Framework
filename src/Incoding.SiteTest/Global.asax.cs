namespace Incoding.SiteTest
{
    #region << Using >>

    using System.Globalization;
    using System.Threading;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using FluentValidation.Mvc;
    using Incoding.Block.ExceptionHandling;
    using Incoding.MvcContrib;
    using Incoding.SiteTest.App_Start;
    using Incoding.SiteTest.Contrib;

    #endregion

    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {            
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            ControllerBuilder.Current.SetControllerFactory(new IncControllerFactory());
            ModelValidatorProviders.Providers.Add(new FluentValidationModelValidatorProvider(new IncValidatorFactory()));
            IncodingSiteBootstrapped.Start();

        }

        protected void Application_Error()
        {
            var lastEx = this.Server.GetLastError();
            ExceptionHandlingFactory.Instance.Handler(lastEx);
        }
    }
}