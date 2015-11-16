namespace Incoding.UnitTest.MvcContribGroup
{
    using Incoding.MSpecContrib;
    using Incoding.MvcContrib;
    using Incoding.MvcContrib.MVD;
    using Machine.Specifications;

    [Subject(typeof(DispatcherControllerBase))]
    public class When_base_dispatcher_controller_query_only_validate_error : Context_dispatcher_controller
    {
        Establish establish = () =>
                              {
                                  Establish(types: new[] { typeof(ShareQuery) });
                                  controller.ModelState.AddModelError("Fake", "Error");
                              };

        Because of = () => { result = controller.Query(typeof(ShareQuery).Name, false, incOnlyValidate: true); };

        It should_be_error = () => result.ShouldEqualWeak(IncodingResult.Error(controller.ModelState));
    }
}