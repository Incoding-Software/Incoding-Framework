namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System;
    using System.Collections.Specialized;
    using System.IO;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Incoding.CQRS;
    using Incoding.MSpecContrib;
    using Incoding.MvcContrib.MVD;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(DispatcherControllerBase))]
    public class When_base_dispatcher_controller_render_without_ajax : Context_dispatcher_controller_render
    {
        private static ShareQuery model;

        Establish establish = () =>
                              {
                                  model = Pleasure.Generator.Invent<ShareQuery>();

                                  controller.ControllerContext = new ControllerContext(Pleasure.MockAsObject<HttpContextBase>(mock =>
                                                                                                                              {
                                                                                                                                  mock.SetupGet(r => r.Request).Returns(Pleasure.Mock<HttpRequestBase>(s => { s.SetupGet(r => r.Headers).Returns(new NameValueCollection()); }).Object);
                                                                                                                                  mock.SetupGet(r => r.Response).Returns(responseBase.Object);
                                                                                                                              }), new RouteData(), controller);

                                  dispatcher.StubQuery(Pleasure.Generator.Invent<CreateByTypeQuery>(dsl => dsl.Tuning(r => r.ControllerContext, controller.ControllerContext)
                                                                                                              .Tuning(r => r.ModelState, controller.ModelState)
                                                                                                              .Empty(r => r.IsGroup)
                                                                                                              .Empty(r => r.IsModel)
                                                                                                              .Tuning(r => r.Type, typeof(ShareQuery).Name)), (object)model);
                                  dispatcher.StubQuery(Pleasure.Generator.Invent<MVDExecute>(dsl => dsl.Tuning(r => r.Instance, new CommandComposite(model))), (object)model);
                                  dispatcher.StubQuery(Pleasure.Generator.Invent<GetMvdParameterQuery>(dsl => dsl.Tuning(r => r.Params, controller.HttpContext.Request.Params)), new GetMvdParameterQuery.Response()
                                                                                                                                                                                 {
                                                                                                                                                                                         Type = typeof(ShareQuery).Name,
                                                                                                                                                                                         View = "View",
                                                                                                                                                                                 });
                              };

        Because of = () => { result = controller.Render(); };

        It should_be_content = () => result.ShouldBeAssignableTo<ContentResult>();

        It should_be_render = () =>
                              {
                                  Action<ViewContext> verify = s => s.ViewData.Model.ShouldEqualWeak(model);
                                  view.Verify(r => r.Render(Pleasure.MockIt.Is(verify), Pleasure.MockIt.IsAny<TextWriter>()));
                                  viewEngines.Verify(r => r.FindPartialView(Pleasure.MockIt.IsAny<ControllerContext>(), "View", Pleasure.MockIt.IsAny<bool>()));
                              };
    }
}