namespace Incoding.UnitTest.MvcContribGroup.Core
{
    #region << Using >>

    using System.Web;
    using Incoding.CQRS;
    using Incoding.MSpecContrib;
    using Incoding.MvcContrib.MVD;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(MVDExecute))]
    public class When_mvd_execute_message_query_with_inheritance
    {
        Establish establish = () =>
                              {
                                  var fakeQuery = Pleasure.Generator.Invent<FakeInheritanceQuery>();
                                  MVDExecute query = new MVDExecute(Pleasure.MockStrictAsObject<HttpContextBase>())
                                                                        {
                                                                                Instance = new CommandComposite(fakeQuery)
                                                                        };
                                  expected = Pleasure.Generator.TheSameString();

                                  mockQuery = MockQuery<MVDExecute, object>
                                          .When(query);
                              };

        Because of = () => mockQuery.Execute();

        It should_be_result = () => mockQuery.ShouldBeIsResult(expected);

        #region Establish value

        public class FakeInheritanceQuery : FakeQuery { }

        public class FakeQuery : QueryBase<string>
        {
            protected override string ExecuteResult()
            {
                return Pleasure.Generator.TheSameString();
            }
        }

        static MockMessage<MVDExecute, object> mockQuery;

        static string expected;

        #endregion
    }
}