namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System;
    using Incoding.CQRS;
    using Incoding.MSpecContrib;
    using Incoding.MvcContrib;
    using Incoding.MvcContrib.MVD;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(DispatcherControllerBase))]
    public class When_base_dispatcher_controller_push_model_state : Context_dispatcher_controller
    {
        #region Fake classes

        public class FakeModelStateCommand : CommandBase
        {
            ////ncrunch: no coverage start
            protected override void Execute()
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region Establish value

        static FakeModelStateCommand command;

        #endregion

        Establish establish = () =>
                              {
                                  command = Pleasure.Generator.Invent<FakeModelStateCommand>();
                                  Establish(types: new[] { command.GetType() });
                                  controller.ModelState.AddModelError("Fake", "Error");
                              };

        Because of = () => { result = controller.Push(typeof(FakeModelStateCommand).Name); };

        It should_be_push = () => dispatcher.ShouldBePush(command, callCount: 0);

        It should_be_result = () => result.ShouldBeIncodingError();

        It should_be_data = () => result.ShouldEqualWeak(IncodingResult.Error(controller.ModelState));
    }
}