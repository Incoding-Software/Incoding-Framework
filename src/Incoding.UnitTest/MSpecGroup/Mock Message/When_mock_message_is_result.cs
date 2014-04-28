namespace Incoding.UnitTest.MSpecGroup
{
    #region << Using >>

    using Incoding.CQRS;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(MockMessage<,>))]
    public class When_mock_message_is_result
    {
        #region Fake classes

        class FakeMessage : QueryBase<string>
        {
            #region Override

            protected override string ExecuteResult()
            {
                return Pleasure.Generator.TheSameString();
            }

            #endregion
        }

        #endregion

        #region Establish value

        static MockMessage<FakeMessage, string> mockMessage;

        #endregion

        Establish establish = () =>
                                  {
                                      mockMessage = MockQuery<FakeMessage, string>
                                              .When(Pleasure.Generator.Invent<FakeMessage>());
                                  };

        Because of = () => mockMessage.Original.Execute();

        It should_be_is_result = () => mockMessage.ShouldBeIsResult(Pleasure.Generator.TheSameString());

        It should_be_is_result_by_action = () => mockMessage.ShouldBeIsResult(i => i.ShouldEqual(Pleasure.Generator.TheSameString()));
    }
}