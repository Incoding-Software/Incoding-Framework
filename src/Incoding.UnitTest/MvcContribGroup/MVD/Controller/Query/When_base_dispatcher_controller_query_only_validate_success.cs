namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using Incoding.MSpecContrib;
    using Incoding.MvcContrib.MVD;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(DispatcherControllerBase))]
    public class When_base_dispatcher_controller_query_only_validate_success : Context_dispatcher_controller
    {
        #region Establish value

        static string queryResult;

        #endregion

        Establish establish = () =>
                              {
                                  var res = Pleasure.Generator.Invent<ShareQuery>();
                                  dispatcher.StubQuery(Pleasure.Generator.Invent<CreateByTypeQuery>(dsl => dsl.Tuning(r => r.Type, typeof(ShareQuery).Name)), (object)res);
                                  queryResult = Pleasure.Generator.String();
                                  dispatcher.StubQuery(new ShareQuery(), queryResult);
                                  dispatcher.StubQuery(Pleasure.Generator.Invent<GetMvdParameterQuery>(dsl => dsl.Tuning(r => r.Params, controller.HttpContext.Request.Params)), new GetMvdParameterQuery.Response()
                                                                                                                                                                                 {
                                                                                                                                                                                         Type = typeof(ShareQuery).Name,
                                                                                                                                                                                         OnlyValidate = true
                                                                                                                                                                                 });
                              };

        Because of = () => { result = controller.Query(); };

        It should_be_result = () => result.ShouldBeIncodingDataIsNull();

        It should_be_success = () => result.ShouldBeIncodingSuccess();
    }
}