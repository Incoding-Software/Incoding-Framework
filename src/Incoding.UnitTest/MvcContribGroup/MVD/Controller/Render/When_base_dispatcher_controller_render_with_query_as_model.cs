﻿namespace Incoding.UnitTest.MvcContribGroup
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
    public class When_base_dispatcher_controller_render_with_query_as_model : Context_dispatcher_controller_render
    {
        Establish establish = () =>
                              {
                                  var query = Pleasure.Generator.Invent<FakeRenderQuery>();
                                  dispatcher.StubQuery(Pleasure.Generator.Invent<CreateByTypeQuery>(dsl => dsl.Tuning(r => r.ControllerContext, controller.ControllerContext)
                                                                                                              .Tuning(r => r.ModelState, controller.ModelState)
                                                                                                              .Empty(r => r.IsGroup)
                                                                                                              .Tuning(r => r.IsModel, true)
                                                                                                              .Tuning(r => r.Type, typeof(FakeRenderQuery).Name)), (object)query);
                                  dispatcher.StubQuery(query, new FakeAppModel());
                                  dispatcher.StubQuery(Pleasure.Generator.Invent<GetMvdParameterQuery>(dsl => dsl.Tuning(r => r.Params, controller.HttpContext.Request.Params)), new GetMvdParameterQuery.Response()
                                                                                                                                                                                 {
                                                                                                                                                                                         Type = typeof(FakeRenderQuery).Name,
                                                                                                                                                                                         IsModel = true,
                                                                                                                                                                                         View = "View"
                                                                                                                                                                                 });
                              };

        Because of = () => { result = controller.Render(); };

        It should_be_render = () =>
                              {
                                  Action<ViewContext> verify = s => s.ViewData.Model.ShouldBeAssignableTo<FakeRenderQuery>();
                                  view.Verify(r => r.Render(Pleasure.MockIt.Is(verify), Pleasure.MockIt.IsAny<TextWriter>()));
                                  viewEngines.Verify(r => r.FindPartialView(Pleasure.MockIt.IsAny<ControllerContext>(), "View", Pleasure.MockIt.IsAny<bool>()));
                              };

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
    }
}