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
    public class When_base_dispatcher_controller_render_with_model : Context_dispatcher_controller_render
    {
        Because of = () =>
                     {
                         var res = Pleasure.Generator.Invent<FakeModel>();
                         dispatcher.StubQuery(Pleasure.Generator.Invent<CreateByTypeQuery>(dsl => dsl.Tuning(r => r.ControllerContext, controller.ControllerContext)
                                                                                                     .Tuning(r => r.ModelState, controller.ModelState)
                                                                                                     .Tuning(r => r.IsModel, true)
                                                                                                     .Empty(r => r.IsGroup)
                                                                                                     .Tuning(r => r.Type, typeof(FakeModel).Name)), (object)res);
                         result = controller.Render("View", typeof(FakeModel).Name, true, false);
                     };

        It should_be_render = () =>
                              {
                                  Action<ViewContext> verify = s => s.ViewData.Model.ShouldBeAssignableTo<FakeModel>();
                                  view.Verify(r => r.Render(Pleasure.MockIt.Is(verify), Pleasure.MockIt.IsAny<TextWriter>()));
                                  viewEngines.Verify(r => r.FindPartialView(Pleasure.MockIt.IsAny<ControllerContext>(), "View", Pleasure.MockIt.IsAny<bool>()));
                              };
    }
}