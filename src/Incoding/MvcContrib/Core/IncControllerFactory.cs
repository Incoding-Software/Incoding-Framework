namespace Incoding.MvcContrib
{
    #region << Using >>

    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Incoding.Block.IoC;
    using JetBrains.Annotations;

    #endregion

    [ExcludeFromCodeCoverage, UsedImplicitly, Obsolete("On next version should be removed. Please do not use IoC on ctor controller")]
    public class IncControllerFactory : DefaultControllerFactory
    {
        #region Fields

        readonly string[] rootNamespaces = new string[] { };

        #endregion

        #region Override

        ////ncrunch: no coverage start
        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {            
            if (controllerType == null)
            {
                var originalNamespace = requestContext.RouteData.DataTokens["Namespaces"];
                var originalArea = requestContext.RouteData.DataTokens["area"];

                requestContext.RouteData.DataTokens["Namespaces"] = this.rootNamespaces;
                requestContext.RouteData.DataTokens["area"] = "";
                controllerType = GetControllerType(requestContext, requestContext.RouteData.Values["controller"].ToString());
                requestContext.RouteData.DataTokens["Namespaces"] = originalNamespace;
                requestContext.RouteData.DataTokens["area"] = originalArea;
            }

            return IoCFactory.Instance.Resolve<IController>(controllerType);
        }

        ////ncrunch: no coverage end

        #endregion

        #region Constructors

        public IncControllerFactory() { }

        public IncControllerFactory(string[] rootNamespaces)
        {
            this.rootNamespaces = rootNamespaces;
        }

        #endregion
    }
}