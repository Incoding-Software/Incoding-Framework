namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System;
    using System.Web;
    using Incoding.CQRS;
    using Incoding.MSpecContrib;
    using Incoding.MvcContrib.MVD;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(DispatcherControllerBase))]
    public class When_base_dispatcher_controller_query : Context_dispatcher_controller
    {
        #region Fake classes

        public class FakeByNameQuery : QueryBase<string>
        {
            ////ncrunch: no coverage start
            protected override string ExecuteResult()
            {
                throw new NotImplementedException();
            }

            ////ncrunch: no coverage end        
        }

        #endregion

        #region Establish value

        static string queryResult;

        #endregion

        Establish establish = () =>
                                  {
                                      Establish(types: new[] { typeof(FakeByNameQuery) });
                                      queryResult = Pleasure.Generator.String();
                                      dispatcher.StubQuery(new FakeByNameQuery(), queryResult);
                                  };

        Because of = () => { result = controller.Query(HttpUtility.UrlEncode(typeof(FakeByNameQuery).Name), false); };

        It should_be_result = () => result.ShouldBeIncodingSuccess<string>(s => s.ShouldEqual(queryResult));
    }
}