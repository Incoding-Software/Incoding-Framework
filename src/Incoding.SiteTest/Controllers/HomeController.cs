namespace Incoding.SiteTest.Controllers
{
    #region << Using >>

    using System.Web.Mvc;
    using Incoding.MvcContrib;

    #endregion

    public class HomeController : IncControllerBase
    {
        #region Api Methods

        public ActionResult Index()
        {
            return View();
        }

        #endregion
    }
}