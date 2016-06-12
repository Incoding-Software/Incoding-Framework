namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using Incoding.CQRS;
    using Incoding.MSpecContrib;
    using Incoding.MvcContrib.MVD;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(DispatcherControllerBase))]
    public class When_base_dispatcher_controller_query : Context_dispatcher_controller
    {
        #region Establish value

        static string queryResult;

        #endregion

        Establish establish = () =>
                              {
                                  var query = Pleasure.Generator.Invent<ShareQuery>();
                                  dispatcher.StubQuery(Pleasure.Generator.Invent<CreateByTypeQuery>(dsl => dsl.Tuning(r => r.ControllerContext, controller.ControllerContext)
                                                                                                              .Tuning(r => r.ModelState, controller.ModelState)
                                                                                                              .Tuning(r => r.IsGroup, false)
                                                                                                              .Tuning(r => r.IsModel, false)
                                                                                                              .Tuning(r => r.Type, typeof(ShareQuery).Name)), (object)query);
                                  queryResult = Pleasure.Generator.String();
                                  dispatcher.StubQuery(Pleasure.Generator.Invent<GetMvdParameterQuery>(dsl => dsl.Tuning(r => r.Params, controller.HttpContext.Request.Params)), new GetMvdParameterQuery.Response()
                                                                                                                                                                                 {
                                                                                                                                                                                         Type = typeof(ShareQuery).Name
                                                                                                                                                                                 });
                                  dispatcher.StubQuery(Pleasure.Generator.Invent<MVDExecute>(dsl => dsl.Tuning(r => r.Instance, new CommandComposite(query))), (object)queryResult);
                              };

        Because of = () => { result = controller.Query(); };

        It should_be_result = () => result.ShouldBeIncodingSuccess<string>(s => s.ShouldEqual(queryResult));
    }
}