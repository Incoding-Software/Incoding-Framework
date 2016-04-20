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
    public class When_base_dispatcher_controller_push : Context_dispatcher_controller
    {
        #region Establish value

        static FakeCommand command;

        #endregion

        Establish establish = () =>
                              {
                                  command = Pleasure.Generator.Invent<FakeCommand>();
                                  dispatcher.StubQuery(Pleasure.Generator.Invent<CreateByTypeQuery>(dsl => dsl.Tuning(r => r.ModelState, controller.ModelState)
                                                                                                                   .Tuning(r => r.ControllerContext, controller.ControllerContext)
                                                                                                                   .Empty(r => r.IsModel)
                                                                                                                   .Empty(r => r.IsGroup)
                                                                                                                   .Tuning(r => r.Type, typeof(FakeCommand).Name)), (object)command);
                              };

        Because of = () => { result = controller.Push(incTypes: typeof(FakeCommand).Name); };

        It should_be_push = () => dispatcher.ShouldBePush(command);

        It should_be_result = () => result.ShouldBeIncodingSuccess();
    }
}