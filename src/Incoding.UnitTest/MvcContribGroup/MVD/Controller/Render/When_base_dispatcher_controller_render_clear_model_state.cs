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
        Because of = () =>
                         {
                             controller.ModelState.AddModelError(Pleasure.Generator.String(), Pleasure.Generator.String());
                             result = controller.Render("View", string.Empty,false);
                         };

        It should_be_clear_model_state = () => controller.ModelState.ShouldBeEmpty();
    }
}