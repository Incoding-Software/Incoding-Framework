namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System;
    using System.Collections.Specialized;
    using System.IO;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Incoding.MSpecContrib;
    using Incoding.MvcContrib.MVD;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(DispatcherControllerBase))]
    public class When_base_dispatcher_controller_render_without_ajax : Context_dispatcher_controller_render
    {
        private static FakeModel model;

        Because of = () =>
                     {
                         model = Pleasure.Generator.Invent<FakeModel>();


                         controller.ControllerContext = new ControllerContext(Pleasure.MockAsObject<HttpContextBase>(mock =>
                                                                                                                           {
                                                                                                                               mock.SetupGet(r => r.Request).Returns(Pleasure.Mock<HttpRequestBase>(s => { s.SetupGet(r => r.Headers).Returns(new NameValueCollection()); }).Object);
                                                                                                                               mock.SetupGet(r => r.Response).Returns(responseBase.Object);
                                                                                                                           }), new RouteData(), controller);

                         dispatcher.StubQuery(Pleasure.Generator.Invent<CreateByTypeQuery>(dsl => dsl.Tuning(r => r.ControllerContext, controller.ControllerContext)
                                                                            .Tuning(r => r.ModelState, controller.ModelState)
                                                                            .Empty(r => r.IsGroup)
                                                                            .Empty(r => r.IsModel)
                                                                            .Tuning(r => r.Type, typeof(FakeModel).Name)), (object)model);
                         dispatcher.StubQuery(Pleasure.Generator.Invent<ExecuteQuery>(dsl => dsl.Tuning(r => r.Instance, model)), (object)model);

                         result = controller.Render("View", typeof(FakeModel).Name, null, false);
                     };

        It should_be_content = () => result.ShouldBeAssignableTo<ContentResult>();

        It should_be_render = () =>
                              {
                                  Action<ViewContext> verify = s => s.ViewData.Model.ShouldEqualWeak(model);
                                  view.Verify(r => r.Render(Pleasure.MockIt.Is(verify), Pleasure.MockIt.IsAny<TextWriter>()));
                                  viewEngines.Verify(r => r.FindPartialView(Pleasure.MockIt.IsAny<ControllerContext>(), "View", Pleasure.MockIt.IsAny<bool>()));
                              };
    }
}