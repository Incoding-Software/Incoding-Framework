namespace $rootnamespace$.Controllers
{    
	using Incoding.MvcContrib;

    public class HomeController : IncControllerBase
    {

	    public HomeController(IDispatcher dispatcher)
                : base(dispatcher) { }

        [HttpGet]
        public ActionResult Index()
        {
		    return View()
		}

    }
}