namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System;
    using Incoding.CQRS;
    using Incoding.MSpecContrib;
    using Incoding.MvcContrib.MVD;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(DispatcherControllerBase))]
    public class When_base_dispatcher_controller_render_enable_validate : Context_dispatcher_controller_render
    {
        Establish establish = () =>
                              {
                                  var res = Pleasure.Generator.Invent<FakeRenderQuery>();
                                  controller.ModelState.AddModelError("Fake", "Error");
                                  dispatcher.StubQuery(Pleasure.Generator.Invent<GetMvdParameterQuery>(dsl => dsl.Tuning(r => r.Params, controller.HttpContext.Request.Params)), new GetMvdParameterQuery.Response()
                                                                                                                                                                                 {
                                                                                                                                                                                         Type = typeof(FakeRenderQuery).Name,
                                                                                                                                                                                         View = "View",
                                                                                                                                                                                         IsValidate = true
                                                                                                                                                                                 });
                                  dispatcher.StubQuery(Pleasure.Generator.Invent<CreateByTypeQuery>(dsl => dsl.Tuning(r => r.ControllerContext, controller.ControllerContext)
                                                                                                              .Tuning(r => r.ModelState, controller.ModelState)
                                                                                                              .Empty(r => r.IsGroup)
                                                                                                              .Tuning(r => r.IsModel, false)
                                                                                                              .Tuning(r => r.Type, typeof(FakeRenderQuery).Name)), (object)res);
                              };

        Because of = () => { result = controller.Render(); };

        It should_be_error = () => { result.ShouldBeIncodingError(); };

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