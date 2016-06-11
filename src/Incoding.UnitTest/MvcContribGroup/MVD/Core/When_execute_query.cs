namespace Incoding.UnitTest.MvcContribGroup.Core
{
    #region << Using >>

    using Incoding.CQRS;
    using Incoding.MSpecContrib;
    using Incoding.MvcContrib.MVD;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(ExecuteQuery))]
    public class When_execute_query
    {
        Establish establish = () =>
                              {
                                  var fakeQuery = Pleasure.Generator.Invent<FakeQuery>();
                                  ExecuteQuery query = Pleasure.Generator.Invent<ExecuteQuery>(dsl => dsl.Tuning(r => r.Instance, fakeQuery));
                                  expected = Pleasure.Generator.TheSameString();

                                  mockQuery = MockQuery<ExecuteQuery, object>
                                          .When(query)
                                          .StubQuery(fakeQuery, expected);
                              };

        Because of = () => mockQuery.Execute();

        It should_be_performance = () => { Pleasure.Do(i => mockQuery.Execute(), 1000).ShouldBeLessThan(30); };

        It should_be_result = () => mockQuery.ShouldBeIsResult(expected);

        #region Establish value

        public class FakeQuery : QueryBase<object>
        {
            protected override object ExecuteResult()
            {
                return Pleasure.Generator.TheSameString();
            }
        }

        static MockMessage<ExecuteQuery, object> mockQuery;

        static object expected;

        #endregion
    }
}