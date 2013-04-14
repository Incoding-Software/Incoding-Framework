namespace Incoding.SiteTest.Controllers
{
    #region << Using >>

    using System.Web.Mvc;
    using Incoding.CQRS;
    using Incoding.MvcContrib;

    #endregion

    public class HomeController : IncControllerBase
    {
        #region Constructors

        public HomeController(IDispatcher dispatcher)
                : base(dispatcher) { }

        #endregion

        #region Api Methods

        public ActionResult Index()
        {
            return View();
        }

        #endregion
    }
}