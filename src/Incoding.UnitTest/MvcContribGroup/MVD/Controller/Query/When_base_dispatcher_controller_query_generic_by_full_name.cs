namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System;
    using System.Web;
    using Incoding.CQRS;
    using Incoding.Extensions;
    using Incoding.MSpecContrib;
    using Incoding.MvcContrib.MVD;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(DispatcherControllerBase))]
    public class When_base_dispatcher_controller_query_generic_by_full_name : Context_dispatcher_controller
    {
        #region Establish value

        static string queryResult;

        #endregion

        Establish establish = () =>
                              {
                                  Establish(types: new[] { typeof(FakeGenericByFullNameGQuery<string>) });
                                  queryResult = Pleasure.Generator.String();
                                  dispatcher.StubQuery(new FakeGenericByFullNameGQuery<string>(), queryResult);
                              };

        Because of = () => { result = controller.Query("{0}|{1}".F(typeof(FakeGenericByFullNameGQuery<>).FullName, HttpUtility.UrlEncode(typeof(string).FullName)), false); };

        It should_be_result = () => result.ShouldBeIncodingSuccess<string>(s => s.ShouldEqual(queryResult));

        #region Fake classes

        public class FakeGenericByFullNameGQuery<T> : QueryBase<T>
        {
            ////ncrunch: no coverage start
            protected override T ExecuteResult()
            {
                throw new NotImplementedException();
            }
        }

        #endregion
    }
}