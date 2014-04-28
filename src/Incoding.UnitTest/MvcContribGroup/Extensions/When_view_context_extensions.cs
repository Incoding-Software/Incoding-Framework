namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System.IO;
    using System.Text;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Incoding.MvcContrib;
    using Machine.Specifications;
    using Moq;
    using It = Machine.Specifications.It;

    #endregion

    [Subject(typeof(ViewContextExtensions))]
    public class When_view_context_extensions
    {
        #region Establish value

        protected static RouteData routeData;

        protected static ViewContext viewContext;

        #endregion

        Establish establish = () =>
                                  {
                                      var httpContext = new Mock<HttpContextBase>();
                                      routeData = new RouteData();

                                      var controllerBase = new Mock<ControllerBase>();
                                      var controllerContext = new ControllerContext(httpContext.Object, routeData, controllerBase.Object);
                                      var view = new Mock<IView>();
                                      viewContext = new ViewContext(controllerContext, view.Object, new ViewDataDictionary(), new TempDataDictionary(), new StringWriter(new StringBuilder()));
                                  };

        Because be = () =>
                         {
                             routeData.DataTokens.Add("action", "IsAction");
                             routeData.DataTokens.Add("area", "IsArea");
                             routeData.DataTokens.Add("controller", "IsController");
                         };

        // ReSharper disable Mvc.ActionNotResolved
        It should_be_action = () => viewContext.IsAction("IsAction").ShouldBeTrue();

        // ReSharper restore Mvc.ActionNotResolved

        // ReSharper disable Mvc.AreaNotResolved
        It should_be_area = () => viewContext.IsArea("IsArea").ShouldBeTrue();

        // ReSharper restore Mvc.AreaNotResolved

        // ReSharper disable Mvc.ControllerNotResolved
        It should_be_controller = () => viewContext.IsController("IsController").ShouldBeTrue();

        // ReSharper restore Mvc.ControllerNotResolved

        // ReSharper disable Mvc.ActionNotResolved
        // ReSharper disable Mvc.ControllerNotResolved
        // ReSharper disable Mvc.AreaNotResolved
        It should_be_current = () => viewContext.IsCurrent("IsAction", "IsController", "IsArea").ShouldBeTrue();

        // ReSharper restore Mvc.AreaNotResolved
        // ReSharper restore Mvc.ControllerNotResolved
        // ReSharper restore Mvc.ActionNotResolved
    }
}