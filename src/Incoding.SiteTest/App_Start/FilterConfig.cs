namespace Incoding.SiteTest.App_Start
{
    #region << Using >>

    using System.Web.Mvc;

    #endregion

    public class FilterConfig
    {
        #region Factory constructors

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        #endregion
    }
}