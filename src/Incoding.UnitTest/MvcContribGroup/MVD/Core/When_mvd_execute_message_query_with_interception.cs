namespace Incoding.UnitTest.MvcContribGroup.Core
{
    #region << Using >>

    using System.Web;
    using Incoding.CQRS;
    using Incoding.MSpecContrib;
    using Incoding.MvcContrib.MVD;
    using Machine.Specifications;
    using Moq;
    using It = Machine.Specifications.It;

    #endregion

    [Subject(typeof(MVDExecute))]
    public class When_mvd_execute_message_query_with_interception
    {
        Establish establish = () =>
                              {
                                  expected = Pleasure.Generator.TheSameString();
                                  var fakeQuery = Pleasure.Generator.Invent<FakeQuery>();
                                  interception = Pleasure.MockStrict<IMessageInterception>(mock =>
                                                                                           {
                                                                                               mock.Setup(r => r.OnBefore(Pleasure.MockIt.IsStrong(fakeQuery), Pleasure.MockIt.IsAny<HttpContextBase>()));
                                                                                               mock.Setup(r => r.OnAfter(Pleasure.MockIt.IsStrong(fakeQuery), Pleasure.MockIt.IsAny<HttpContextBase>()));
                                                                                           });
                                  MVDExecute.SetInterception(() => interception.Object);

                                  var commandComposite = new CommandComposite(fakeQuery);
                                  MVDExecute query = new MVDExecute(Pleasure.MockStrictAsObject<HttpContextBase>())
                                                          {
                                                                  Instance = commandComposite
                                                          };

                                  mockQuery = MockQuery<MVDExecute, object>
                                          .When(query);
                              };

        Because of = () => mockQuery.Execute();

        It should_be_result = () => mockQuery.ShouldBeIsResult(expected);

        It should_be_verify = () => interception.VerifyAll();

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

        private static Mock<IMessageInterception> interception;

        #endregion
    }
}