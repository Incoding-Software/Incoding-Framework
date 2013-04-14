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

    [ExcludeFromCodeCoverage, UsedImplicitly]
    public class IncControllerFactory : DefaultControllerFactory
    {
        #region Override

        ////ncrunch: no coverage start
        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            return IoCFactory.Instance.Resolve<IController>(controllerType);
        }

        ////ncrunch: no coverage end
        #endregion
    }
}