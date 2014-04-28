namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System;
    using System.IO;
    using System.Web.Mvc;
    using Incoding.CQRS;
    using Incoding.MSpecContrib;
    using Incoding.MvcContrib.MVD;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(DispatcherControllerBase))]
    public class When_base_dispatcher_controller_render_with_query : Context_dispatcher_controller_render
    {
        #region Fake classes

        public class FakeAppModel { }

        public class FakeRenderQuery : QueryBase<FakeAppModel>
        {
            ////ncrunch: no coverage start
            protected override FakeAppModel ExecuteResult()
            {
                throw new NotImplementedException();
            }

            ////ncrunch: no coverage end        
        }

        #endregion

        Establish establish = () =>
                                  {
                                      Establish(types: new[] { typeof(FakeRenderQuery) });
                                      dispatcher.StubQuery(new FakeRenderQuery(), new FakeAppModel());
                                  };

        Because of = () => { result = controller.Render("View", typeof(FakeRenderQuery).FullName, string.Empty,false); };

        It should_be_render = () =>
                                  {
                                      Action<ViewContext> verify = s => s.ViewData.Model.ShouldBeOfType<FakeAppModel>();
                                      view.Verify(r => r.Render(Pleasure.MockIt.Is(verify), Pleasure.MockIt.IsAny<TextWriter>()));
                                      viewEngines.Verify(r => r.FindPartialView(Pleasure.MockIt.IsAny<ControllerContext>(), "View", Pleasure.MockIt.IsAny<bool>()));
                                  };
    }
}