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
                                  dispatcher.StubQuery(Pleasure.Generator.Invent<CreateByTypeQuery.AsCommands>(dsl => dsl.Tuning(r => r.ModelState, controller.ModelState)
                                                                                                                         .Tuning(r => r.ControllerContext, controller.ControllerContext)
                                                                                                                         .Tuning(r => r.IsComposite, false)
                                                                                                                         .Tuning(r => r.IncTypes, typeof(FakeCommand).Name)), new CommandBase[] { command });
                              };

        Because of = () => { result = controller.Push(incTypes: typeof(FakeCommand).Name); };

        It should_be_push = () => dispatcher.ShouldBePush(command);

        It should_be_result = () => result.ShouldBeIncodingSuccess();
    }
}