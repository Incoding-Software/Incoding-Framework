namespace Incoding.UnitTest
{
    using Incoding.CQRS;
    using Incoding.Data;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    [Subject(typeof(DeleteEntityByIdCommand<>))]
    public class When_delete_entity_by_id_with_generic
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

        static MockMessage<DeleteEntityByIdCommand<FakeEntity>, object> mockCommand;

        static FakeEntity fakeEntity;

        #endregion

        Establish establish = () =>
                                  {
                                      var command = new DeleteEntityByIdCommand<FakeEntity>(Pleasure.Generator.String());
                                      fakeEntity = Pleasure.Generator.Invent<FakeEntity>();

                                      mockCommand = MockCommand<DeleteEntityByIdCommand<FakeEntity>>
                                              .When(command)
                                              .StubGetById(command.Id, fakeEntity)
                                              .StubPublish<OnBeforeDeleteEntityEvent>(@event => @event.Entity.ShouldEqualWeak(fakeEntity));
                                  };

        Because of = () => mockCommand.Original.Execute();

        It should_be_delete = () => mockCommand.ShouldBeDelete(fakeEntity);
    }
}