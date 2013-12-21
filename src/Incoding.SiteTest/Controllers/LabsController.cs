namespace Incoding.SiteTest.Controllers
{
    #region << Using >>

    using System.Collections.Generic;
    using System.Web.Mvc;
    using Incoding.Extensions;
    using Incoding.MvcContrib;
    using Incoding.SiteTest.VM;

    #endregion

    public class LabsController : IncControllerBase
    {
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
            return View(new LabsIndexContainer
                            {
                                    DropId = LabsIndexContainer.TestEnum.Value2
                            });
        }

        [HttpPost]
        public ActionResult Submit(string value, string optional)
        {
            return IncJson(new KeyValueVm());
        }

        #endregion

        #region Api Methods

        public ActionResult FetchForDd()
        {
            var optGroupVm = typeof(LabsIndexContainer.TestEnum)
                    .ToKeyValueVm()
                    .ToOptGroup();
            return IncJson(optGroupVm);
        }

        public ActionResult GetVal(string original)
        {
            return IncJson(original + original);
        }

        #endregion

    }
}