namespace Incoding.UnitTest
{
    using Incoding.Block;
    using Incoding.MSpecContrib;
    using Machine.Specifications;
    using It = Machine.Specifications.It;

    [Subject(typeof(ChangeDelayToSchedulerStatusByUIDCommand))]
    public class When_change_delay_to_scheduler_status_by_uid   
    {
        #region Establish value

        static MockMessage<ChangeDelayToSchedulerStatusByUIDCommand, object> mockCommand;

        #endregion

        Establish establish = () =>
                                  {
                                      ChangeDelayToSchedulerStatusByUIDCommand command = Pleasure.Generator.Invent<ChangeDelayToSchedulerStatusByUIDCommand>();

                                      DelayToScheduler[] entities = new[]
                                                                                 {
                                                                                         Pleasure.Generator.Invent<DelayToScheduler>(), Pleasure.Generator.Invent<DelayToScheduler>(),
                                                                                 };
                                      mockCommand = MockCommand<ChangeDelayToSchedulerStatusByUIDCommand>
                                              .When(command)
                                              .StubQuery(whereSpecification: new DelayToSchedulerByUIDWhereSpec(command.UID),
                                                         entities: entities)
                                              .StubPush(new ChangeDelayToSchedulerStatusCommand()
                                                                {
                                                                        Ids = new[] { entities[0].Id, entities[1].Id },
                                                                        Status = command.Status
                                                                });

                                  };

        Because of = () => mockCommand.Original.Execute();

        It should_be_verify = () => mockCommand.ShouldBePushed();
    }
}