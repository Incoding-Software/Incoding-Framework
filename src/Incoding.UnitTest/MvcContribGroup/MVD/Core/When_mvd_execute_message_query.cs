namespace Incoding.UnitTest.MvcContribGroup.Core
{
    #region << Using >>

    using System.Web;
    using Incoding.CQRS;
    using Incoding.MSpecContrib;
    using Incoding.MvcContrib.MVD;
    using Machine.Specifications;
    using It = Machine.Specifications.It;

    #endregion

    [Subject(typeof(MVDExecute))]
    public class When_mvd_execute_message_query
    {
        Establish establish = () =>
                              {
                                  expected = Pleasure.Generator.TheSameString();
                                  var fakeQuery = Pleasure.Generator.Invent<FakeQuery>();
                                  MVDExecute query = new MVDExecute(Pleasure.MockStrictAsObject<HttpContextBase>())
                                                                        {
                                                                                Instance = new CommandComposite(fakeQuery)
                                                                        };

                                  mockQuery = MockQuery<MVDExecute, object>
                                          .When(query);
                              };

        Because of = () => mockQuery.Execute();

        It should_be_performance = () => { Pleasure.Do(i => mockQuery.Execute(), 1000).ShouldBeLessThan(25); };

        It should_be_result = () => mockQuery.ShouldBeIsResult(expected);

        #region Establish value

        public class FakeQuery : CommandBase
        {
            protected override void Execute()
            {
                Result = Pleasure.Generator.TheSameString();
            }
        }

        static MockMessage<MVDExecute, object> mockQuery;

        static object expected;

        #endregion
    }
}