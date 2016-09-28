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
    public class When_base_dispatcher_controller_query_to_file_with_null : Context_dispatcher_controller
    {
        private static Exception exception;

        Establish establish = () =>
                              {
                                  var query = Pleasure.Generator.Invent<FakeFileByNameQuery>();
                                  dispatcher.StubQuery(Pleasure.Generator.Invent<CreateByTypeQuery>(dsl => dsl.Tuning(r => r.ControllerContext, controller.ControllerContext)
                                                                                                              .Tuning(r => r.ModelState, controller.ModelState)
                                                                                                              .Empty(r => r.IsModel)
                                                                                                              .Empty(r => r.IsGroup)
                                                                                                              .Tuning(r => r.Type, typeof(FakeFileByNameQuery).Name)), (object)query);
                                  Byte[] content = null;

                                  dispatcher.StubQuery(Pleasure.Generator.Invent<MVDExecute>(dsl => dsl.Tuning(r => r.Instance, new CommandComposite(query))), (object)content);
                                  dispatcher.StubQuery(Pleasure.Generator.Invent<GetMvdParameterQuery>(dsl => dsl.Tuning(r => r.Params, controller.HttpContext.Request.Params)), new GetMvdParameterQuery.Response() { Type = typeof(FakeFileByNameQuery).Name, });
                              };

        Because of = () => { exception = Catch.Exception(() => controller.QueryToFile()); };

        It should_be_exception = () => { exception.Message.ShouldEqual(@"Result from query FakeFileByNameQuery is null but argument 'result' should be not null
Parameter name: result"); };
    }
}