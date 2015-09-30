namespace Incoding.UnitTest
{
    #region << Using >>

    using Incoding.CQRS;
    using Incoding.Data;
    using Incoding.MSpecContrib;
    using Machine.Specifications;
    using Moq;
    using It = Machine.Specifications.It;

    #endregion

    [Subject(typeof(MessageBase<>))]
    public class When_message_base_repository_once
    {
        #region Fake classes

        public class FakeCommand : MessageBase<object>
        {
            protected override void Execute()
            {
                Pleasure.Do10(i => Repository.Delete<FakeEntityForNew>(i));
            }
        }

        #endregion

        #region Establish value

        static Mock<IRepository> repository;

        static Mock<IUnitOfWork> unitOfWork;

        static FakeCommand message;

        #endregion

        Establish establish = () =>
                              {
                                  message = Pleasure.Generator.Invent<FakeCommand>(dsl => dsl.GenerateTo(r => r.Setting));
                                  repository = Pleasure.MockStrict<IRepository>(mock => mock.Setup(r => r.Delete<FakeEntityForNew>(Pleasure.MockIt.IsAny<int>())));
                                  unitOfWork = Pleasure.MockStrict<IUnitOfWork>(mock => mock.Setup(r => r.GetRepository()).Returns(repository.Object));
                              };

        Because of = () => message.OnExecute(Pleasure.MockStrictAsObject<IDispatcher>(), unitOfWork.Object);

        It should_be_delete = () => repository.Verify(r => r.Delete<FakeEntityForNew>(Pleasure.MockIt.IsAny<int>()), Times.Exactly(10));

        It should_be_set_once = () => unitOfWork.Verify(work => work.GetRepository(), Times.Once());
    }
}