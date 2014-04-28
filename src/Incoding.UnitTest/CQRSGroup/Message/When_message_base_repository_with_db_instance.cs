namespace Incoding.UnitTest
{
    #region << Using >>

    using Incoding.Block.IoC;
    using Incoding.CQRS;
    using Incoding.Data;
    using Incoding.MSpecContrib;
    using Machine.Specifications;
    using Moq;
    using It = Machine.Specifications.It;

    #endregion

    [Subject(typeof(MessageBase<>))]
    public class When_message_base_repository_with_db_instance
    {
        #region Fake classes

        class FakeMessage : MessageBase<object>
        {
            public override void Execute()
            {
                Repository.Flush();
            }
        }

        #endregion

        #region Establish value

        static FakeMessage message;

        static Mock<IRepository> repository;

        #endregion

        Establish establish = () =>
                                  {
                                      string named = Pleasure.Generator.String();
                                      repository = Pleasure.Mock<IRepository>();
                                      IoCFactory.Instance.StubTryResolveByNamed(named, repository.Object);
                                      message = new FakeMessage
                                                    {
                                                            Setting = new MessageExecuteSetting
                                                                          {
                                                                                  DataBaseInstance = named
                                                                          }
                                                    };
                                  };

        Because of = () => message.Execute();

        It should_be_open_once = () => repository.Verify(r => r.Flush());
    }
}