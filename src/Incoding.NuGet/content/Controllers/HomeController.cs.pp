namespace $rootnamespace$.Controllers
{    
	using Incoding.MvcContrib;
	using Incoding;

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