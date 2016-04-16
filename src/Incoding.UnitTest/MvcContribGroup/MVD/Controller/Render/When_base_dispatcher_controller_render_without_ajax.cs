namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System;
    using System.Collections.Specialized;
    using System.IO;
    using System.Web;
    using System.Web.Mvc;
    using Incoding.MSpecContrib;
    using Incoding.MvcContrib.MVD;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(DispatcherControllerBase))]
    public class When_base_dispatcher_controller_render_without_ajax : Context_dispatcher_controller_render
    {
        Because of = () =>
                     {
                         var res = Pleasure.Generator.Invent<FakeModel>();
                         dispatcher.StubQuery(Pleasure.Generator.Invent<CreateByTypeQuery>(dsl => dsl.Tuning(r => r.Type, typeof(FakeModel).Name)), (object)res);
                         requestBase = Pleasure.Mock<HttpRequestBase>(mock => { mock.SetupGet(r => r.Headers).Returns(new NameValueCollection()); });
                         result = controller.Render("View", typeof(FakeModel).Name, null,false);
                     };

        It should_be_content = () => result.ShouldBeAssignableTo<ContentResult>();

        It should_be_render = () =>
                              {
                                  Action<ViewContext> verify = s => s.ViewData.Model.ShouldBeAssignableTo<FakeModel>();
                                  view.Verify(r => r.Render(Pleasure.MockIt.Is(verify), Pleasure.MockIt.IsAny<TextWriter>()));
                                  viewEngines.Verify(r => r.FindPartialView(Pleasure.MockIt.IsAny<ControllerContext>(), "View", Pleasure.MockIt.IsAny<bool>()));
                              };
    }
}