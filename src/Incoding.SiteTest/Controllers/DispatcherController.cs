namespace Incoding.SiteTest.Controllers
{
    #region << Using >>

    using System;
    using System.Reflection;
    using Incoding.MvcContrib.MVD;

    #endregion

    public class DispatcherController : DispatcherControllerBase
    {
        public DispatcherController()
                : base(typeof(DispatcherController).Assembly) { }
    }
}