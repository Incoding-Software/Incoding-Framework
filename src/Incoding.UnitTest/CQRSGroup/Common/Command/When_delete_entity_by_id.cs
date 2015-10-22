namespace Incoding.UnitTest
{
    #region << Using >>

    using Incoding.CQRS;
    using Incoding.Data;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(DeleteEntityByIdCommand))]
    public class When_delete_entity_by_id
    {
        #region Fake classes

        class FakeEntity : IncEntityBase
        {
            #region Properties

            public string Prop { get; set; }

            #endregion
        }

        #endregion

        #region Establish value

        static MockMessage<DeleteEntityByIdCommand, object> mockCommand;

        static FakeEntity fakeEntity;

        #endregion

        Establish establish = () =>
                                  {
                                      var command = new DeleteEntityByIdCommand(Pleasure.Generator.String(), typeof(FakeEntity));
                                      fakeEntity = Pleasure.Generator.Invent<FakeEntity>();

                                      mockCommand = MockCommand<DeleteEntityByIdCommand>
                                              .When(command)
                                              .StubGetById(command.Id, fakeEntity);

                                  };

        Because of = () => mockCommand.Original.Execute();

        It should_be_delete = () => mockCommand.ShouldBeDelete(fakeEntity);
    }
}