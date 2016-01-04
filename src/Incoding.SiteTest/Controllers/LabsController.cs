namespace Incoding.SiteTest.Controllers
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Incoding.MvcContrib;
    using Incoding.SiteTest.VM;

    #endregion

    public class LabsController : IncControllerBase
    {
        #region Http action

        
        public ActionResult FetchCountry()
        {
            return IncJson(new OptGroupVm(new List<KeyValueVm>
                                          {
                                                  new KeyValueVm("Russian"), 
                                                  new KeyValueVm("USA")
                                          }));
        }
        public ActionResult FetchCountryWithSelected()
        {
            return IncJson(new OptGroupVm(new List<KeyValueVm>
                                          {
                                                  new KeyValueVm("Russian"), 
                                                  new KeyValueVm("UK","UK",true), 
                                                  new KeyValueVm("USA")
                                          }));
        }

        [HttpGet]
        public ActionResult Inc_271()
        {
            return View("Issue/inc_271");
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View(new AddProductCommand()
                        {
                                HasValue = "USA"
                        });
        }

        [HttpPost]
        public ActionResult Submit(string value, string text)
        {
            return IncJson(new KeyValueVm(value,text));
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