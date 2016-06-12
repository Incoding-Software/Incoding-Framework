namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using Incoding.CQRS;
    using Incoding.MSpecContrib;
    using Incoding.MvcContrib.MVD;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(DispatcherControllerBase))]
    public class When_base_dispatcher_controller_push_composite_as_array : Context_dispatcher_controller
    {
        static FakeCommand command1;

        static FakeCommand command2;

        private static bool isComposite;

        Establish establish = () =>
                              {
                                  isComposite = Pleasure.Generator.Bool();
                                  command1 = Pleasure.Generator.Invent<FakeCommand>();
                                  command2 = Pleasure.Generator.Invent<FakeCommand>();
                                  dispatcher.StubQuery(Pleasure.Generator.Invent<CreateByTypeQuery.AsCommands>(dsl => dsl.Tuning(r => r.IncTypes, typeof(FakeCommand).Name)
                                                                                                                         .Tuning(r => r.ModelState, controller.ModelState)
                                                                                                                         .Tuning(r => r.ControllerContext, controller.ControllerContext)
                                                                                                                         .Tuning(r => r.IsComposite, isComposite)), new CommandBase[] { command1, command2 });
                                  dispatcher.StubQuery(Pleasure.Generator.Invent<GetMvdParameterQuery>(dsl => dsl.Tuning(r => r.Params, controller.HttpContext.Request.Params)), new GetMvdParameterQuery.Response()
                                                                                                                                                                                 {
                                                                                                                                                                                         Type = typeof(FakeCommand).Name,
                                                                                                                                                                                         IsCompositeArray = isComposite
                                  });
                              };

        Because of = () => { result = controller.Push(); };

        It should_be_push_1 = () => dispatcher.ShouldBePush(command1);

        It should_be_push_2 = () => dispatcher.ShouldBePush(command2);
    }
}