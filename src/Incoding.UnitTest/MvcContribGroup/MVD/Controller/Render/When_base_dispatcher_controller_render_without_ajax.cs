namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System;
    using System.IO;
    using System.Web.Mvc;
    using Incoding.MSpecContrib;
    using Incoding.MvcContrib.MVD;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(DispatcherControllerBase))]
    public class When_base_dispatcher_controller_render_without_ajax : Context_dispatcher_controller_render
    {
        #region Fake classes

        public class FakeRenderModelWithoutAjax { }

        #endregion

        Because of = () =>
                         {
                             Establish(types: new[] { typeof(FakeRenderModelWithoutAjax) }, isAjax: false);
                             result = controller.Render("View", typeof(FakeRenderModelWithoutAjax).Name, null);
                         };

        It should_be_render = () =>
                                  {
                                      Action<ViewContext> verify = s => s.ViewData.Model.ShouldBeOfType<FakeRenderModelWithoutAjax>();
                                      view.Verify(r => r.Render(Pleasure.MockIt.Is(verify), Pleasure.MockIt.IsAny<TextWriter>()));
                                      viewEngines.Verify(r => r.FindPartialView(Pleasure.MockIt.IsAny<ControllerContext>(), "View", Pleasure.MockIt.IsAny<bool>()));
                                  };

        It should_be_content = () => result.ShouldBeOfType<ContentResult>();
    }
}