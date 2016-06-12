namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using Incoding.MSpecContrib;
    using Incoding.MvcContrib.MVD;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(DispatcherControllerBase))]
    public class When_base_dispatcher_controller_validate_success : Context_dispatcher_controller
    {
        #region Establish value

        static FakeCommand command;

        #endregion

        Establish establish = () =>
                              {
                                  command = Pleasure.Generator.Invent<FakeCommand>();
                                  dispatcher.StubQuery(Pleasure.Generator.Invent<CreateByTypeQuery>(dsl => dsl.Tuning(r => r.Type, typeof(FakeCommand).Name)), (object)command);
                                  dispatcher.StubQuery(Pleasure.Generator.Invent<GetMvdParameterQuery>(dsl => dsl.Tuning(r => r.Params, controller.HttpContext.Request.Params)), new GetMvdParameterQuery.Response()
                                                                                                                                                                                 {
                                                                                                                                                                                         Type = typeof(FakeCommand).Name,                                                                                                                                                                                         
                                                                                                                                                                                 });
                              };

        Because of = () => { result = controller.Validate(); };

        It should_be_push = () => dispatcher.ShouldBePush(command, callCount: 0);

        It should_be_result = () => result.ShouldBeIncodingSuccess();
    }
}