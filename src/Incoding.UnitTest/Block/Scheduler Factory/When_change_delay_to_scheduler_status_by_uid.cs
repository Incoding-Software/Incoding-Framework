namespace Incoding.UnitTest
{
    #region << Using >>

    using System;
    using Incoding.Block;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(ChangeDelayToSchedulerStatusByUIDCommand))]
    public class When_change_delay_to_scheduler_status_by_uid
    {
        #region Establish value

        static MockMessage<ChangeDelayToSchedulerStatusByUIDCommand, object> mockCommand;

        static Exception exception;

        #endregion

        Establish establish = () =>
                              {
                                  var command = Pleasure.Generator.Invent<ChangeDelayToSchedulerStatusByUIDCommand>();

                                  var entities = new[]
                                                 {
                                                         Pleasure.Generator.Invent<DelayToScheduler>(), Pleasure.Generator.Invent<DelayToScheduler>(), 
                                                 };
                                  mockCommand = MockCommand<ChangeDelayToSchedulerStatusByUIDCommand>
                                          .When(command)
                                          .StubQuery(whereSpecification: new DelayToScheduler.Where.ByUID(command.UID), 
                                                     entities: entities)
                                          .StubPush(new ChangeDelayToSchedulerStatusCommand() { Id = entities[0].Id, Status = command.Status })
                                          .StubPush(new ChangeDelayToSchedulerStatusCommand() { Id = entities[1].Id, Status = command.Status });
                              };

        Because of = () => { exception = Catch.Exception(() => mockCommand.Original.Execute()); };

        It should_be_success = () => exception.ShouldBeNull();
    }
}