namespace Incoding.SiteTest.Controllers
{
    #region << Using >>

    using System.Collections.Generic;
    using System.Web.Mvc;
    using Incoding.CQRS;
    using Incoding.MvcContrib;
    using Incoding.SiteTest.VM;

    #endregion

    public class LabsController : IncControllerBase
    {
        #region Constructors

        public LabsController(IDispatcher dispatcher)
                : base(dispatcher) { }

        #endregion

        #region Http action

        [HttpGet]
        public ActionResult FetchCountry()
        {
            return IncJson(new OptGroupVm(new List<KeyValueVm>
                                              {
                                                      new KeyValueVm("Russian"),
                                                      new KeyValueVm("USA")
                                              }));
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View(new LabsIndexContainer()
                            {
                                    DropId = "USA"
                            });
        }

        #endregion
    }
}