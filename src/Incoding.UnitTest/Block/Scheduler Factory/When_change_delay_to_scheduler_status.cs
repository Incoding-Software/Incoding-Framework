namespace Incoding.UnitTest
{
    #region << Using >>

    using System;
    using Incoding.Block;
    using Incoding.CQRS;
    using Incoding.MSpecContrib;
    using Machine.Specifications;
    using Moq;
    using It = Machine.Specifications.It;

    #endregion

    [Subject(typeof(ChangeDelayToSchedulerStatusCommand))]
    public class When_change_delay_to_scheduler_status
    {
        Establish establish = () =>
                              {
                                  var command = Pleasure.Generator.Invent<ChangeDelayToSchedulerStatusCommand>(dsl => dsl.Tuning(s => s.Status, DelayOfStatus.Success));
                                  recurrency = Pleasure.Generator.Invent<GetRecurrencyDateQuery>();
                                  var instance = Pleasure.Generator.Invent<ChangeDelayToSchedulerStatusCommand>();
                                  delay = Pleasure.MockStrict<DelayToScheduler>(mock =>
                                                                                {
                                                                                    lastStartOn = Pleasure.Generator.DateTime();
                                                                                    mock.SetupGet(r => r.StartsOn).Returns(lastStartOn);
                                                                                    mock.SetupSet(r => r.Status = command.Status);
                                                                                    mock.SetupSet(r => r.Description = command.Description);

                                                                                    mock.SetupGet(r => r.Recurrence).Returns(recurrency);
                                                                                    mock.SetupGet(r => r.UID).Returns(Guid.NewGuid().ToString);
                                                                                    mock.SetupGet(r => r.Instance).Returns(instance);
                                                                                });
                                  DateTime? nextDt = Pleasure.Generator.DateTime();
                                  mockCommand = MockCommand<ChangeDelayToSchedulerStatusCommand>
                                          .When(command)
                                          .StubGetById(command.Ids[0], delay.Object)
                                          .StubQuery(recurrency, nextDt)
                                          .StubPush<AddDelayToSchedulerCommand>(dsl => dsl.Tuning(r => r.Recurrency, recurrency)
                                                                                          .Tuning(r => r.UID, delay.Object.UID)
                                                                                          .Tuning(r => r.Commands, Pleasure.ToList(instance as IMessage<object>))
                                                                                          .Tuning(r => r.Setting, instance.Setting));
                              };

        Because of = () => mockCommand.Original.Execute();

        It should_be_update_start_on = () => recurrency.NowDate.ShouldEqual(lastStartOn);

        It should_be_verify = () => delay.VerifyAll();

        #region Establish value

        static MockMessage<ChangeDelayToSchedulerStatusCommand, object> mockCommand;

        static Mock<DelayToScheduler> delay;

        static DateTime lastStartOn;

        static GetRecurrencyDateQuery recurrency;

        #endregion
    }
}