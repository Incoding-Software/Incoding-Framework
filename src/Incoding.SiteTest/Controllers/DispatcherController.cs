namespace Incoding.SiteTest.Controllers
{
    #region << Using >>

    using System;
    using Incoding.MvcContrib.MVD;

    #endregion

    public class DispatcherController : DispatcherControllerBase
    {
        #region Constructors

        ////ncrunch: no coverage start
        public DispatcherController()
                : base(typeof(AddProductCommand).Assembly) { }
        ////ncrunch: no coverage end

        #endregion
    }
}