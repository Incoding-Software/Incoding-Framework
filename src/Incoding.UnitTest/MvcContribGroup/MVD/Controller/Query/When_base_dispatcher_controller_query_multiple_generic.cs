namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System;
    using Incoding.CQRS;
    using Incoding.Data;
    using Incoding.Extensions;
    using Incoding.MSpecContrib;
    using Incoding.MvcContrib.MVD;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(DispatcherControllerBase))]
    public class When_base_dispatcher_controller_query_multiple_generic : Context_dispatcher_controller
    {
        #region Establish value

        static string queryResult;

        #endregion

        Establish establish = () =>
                              {
                                  Establish(types: new[] { typeof(FakeGenericByNameQuery<string, IncEntityBase>) });
                                  queryResult = Pleasure.Generator.String();
                                  dispatcher.StubQuery(new FakeGenericByNameQuery<string, IncEntityBase>(), queryResult);
                              };

        Because of = () => { result = controller.Query("{0}|{1}".F(typeof(FakeGenericByNameQuery<string, IncEntityBase>).Name, "{0}/{1}".F(typeof(string).Name, typeof(IncEntityBase).Name)), false); };

        It should_be_result = () => result.ShouldBeIncodingSuccess<string>(s => s.ShouldEqual(queryResult));

        #region Fake classes

        public class FakeGenericByNameQuery<T, T2> : QueryBase<T>
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