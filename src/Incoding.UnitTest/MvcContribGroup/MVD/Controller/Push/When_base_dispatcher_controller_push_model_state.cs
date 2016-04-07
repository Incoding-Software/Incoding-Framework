namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using Incoding.MSpecContrib;
    using Incoding.MvcContrib;
    using Incoding.MvcContrib.MVD;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(DispatcherControllerBase))]
    public class When_base_dispatcher_controller_push_model_state : Context_dispatcher_controller
    {
        #region Establish value

        static FakeCommand command;

        #endregion

        Establish establish = () =>
                              {
                                  command = Pleasure.Generator.Invent<FakeCommand>();
                                  dispatcher.StubQuery(Pleasure.Generator.Invent<CreateByTypeQuery>(dsl => dsl.Tuning(r => r.ControllerContext, controller.ControllerContext)
                                                                                                              .Tuning(r => r.ModelState, controller.ModelState)
                                                                                                              .Tuning(r => r.IsModel, false)
                                                                                                              .Tuning(r => r.IsGroup, false)
                                                                                                              .Tuning(r => r.Type, typeof(FakeCommand).Name)), (object)command);
                                  controller.ModelState.AddModelError("Fake", "Error");
                              };

        Because of = () => { result = controller.Push(typeof(FakeCommand).Name); };

        It should_be_data = () => result.ShouldEqualWeak(IncodingResult.Error(controller.ModelState));

        It should_be_push = () => dispatcher.ShouldBePush(command, callCount: 0);

        It should_be_result = () => result.ShouldBeIncodingError();
    }
}