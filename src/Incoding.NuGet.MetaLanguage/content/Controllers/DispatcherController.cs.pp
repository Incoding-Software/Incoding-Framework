namespace $rootnamespace$.Controllers
{        
    using Incoding.MvcContrib.MVD;    
	
    public class DispatcherController : DispatcherControllerBase
    {
        public DispatcherController()
                : base(typeof(Bootstrapper).Assembly) { }
    }
}