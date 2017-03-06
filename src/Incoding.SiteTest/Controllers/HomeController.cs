namespace Incoding.SiteTest.Controllers
{
    #region << Using >>

    using System;
    using System.Web.Mvc;
    using Incoding.Block;
    using Incoding.CQRS;
    using Incoding.MvcContrib;

    #endregion

    public class HomeController : IncControllerBase
    {
        #region Api Methods

        public ActionResult Index()
        {
            dispatcher.Push(new AddDelayToSchedulerCommand()
                            {
                                    Command = new AddProductCommand(),
                            });
            dispatcher.Push(new AddDelayToSchedulerCommand()
                            {
                                    Command = new AddProductCommand(),
                                    Recurrency = new GetRecurrencyDateQuery()
                                                 {
                                                         StartDate = DateTime.UtcNow.AddMinutes(15)
                                                 }
                            });
            return View();
        }

        public ActionResult AddNewToScheduler()
        {
            return TryPush(new AddDelayToSchedulerCommand()
                           {
                                   Command = new AddProductCommand(),
                           });
        }

        #endregion
    }
}