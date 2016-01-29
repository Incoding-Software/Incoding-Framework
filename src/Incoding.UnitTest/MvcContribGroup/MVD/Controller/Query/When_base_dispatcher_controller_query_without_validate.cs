namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using Incoding.MSpecContrib;
    using Incoding.MvcContrib.MVD;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(DispatcherControllerBase))]
    public class When_base_dispatcher_controller_query_without_validate : Context_dispatcher_controller
    {
        #region Establish value

        static string queryResult;

        #endregion

        Establish establish = () =>
                              {
                                  var res = Pleasure.Generator.Invent<ShareQuery>();
                                  dispatcher.StubQuery(Pleasure.Generator.Invent<CreateByTypeQuery>(dsl => dsl.Tuning(r => r.Type, typeof(ShareQuery).Name)), (object)res);
                                  controller.ModelState.AddModelError("Fake", "Error");
                              };

        Because of = () => { result = controller.Query(typeof(ShareQuery).FullName, false); };

        It should_be_result = () => result.ShouldBeIncodingSuccess();
    }
}