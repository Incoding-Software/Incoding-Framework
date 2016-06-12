namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using Incoding.MSpecContrib;
    using Incoding.MvcContrib.MVD;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(DispatcherControllerBase))]
    public class When_base_dispatcher_controller_render_clear_model_state : Context_dispatcher_controller_render
    {
        Establish establish = () =>
                              {
                                  dispatcher.StubQuery(Pleasure.Generator.Invent<GetMvdParameterQuery>(dsl => dsl.Tuning(r => r.Params, controller.HttpContext.Request.Params)), new GetMvdParameterQuery.Response()
                                                                                                                                                                                 {
                                                                                                                                                                                         View = "View"
                                                                                                                                                                                 });
                                  controller.ModelState.AddModelError(Pleasure.Generator.String(), Pleasure.Generator.String());
                              };

        Because of = () => { result = controller.Render(); };

        It should_be_clear_model_state = () => controller.ModelState.ShouldBeEmpty();
    }
}