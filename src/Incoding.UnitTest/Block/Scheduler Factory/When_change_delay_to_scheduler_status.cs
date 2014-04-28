namespace Incoding.UnitTest
{
    #region << Using >>

    using Incoding.Block;
    using Incoding.MSpecContrib;
    using Machine.Specifications;
    using Moq;
    using It = Machine.Specifications.It;

    #endregion

    [Subject(typeof(ChangeDelayToSchedulerStatusCommand))]
    public class When_change_delay_to_scheduler_status
    {
        #region Establish value

        static MockMessage<ChangeDelayToSchedulerStatusCommand, object> mockCommand;

        static Mock<DelayToScheduler> delay;

        #endregion

        Establish establish = () =>
                                  {
                                      var command = Pleasure.Generator.Invent<ChangeDelayToSchedulerStatusCommand>();

                                      delay = Pleasure.MockStrict<DelayToScheduler>(mock =>
                                                                                        {
                                                                                            mock.SetupSet(r => r.Status = command.Status);
                                                                                            mock.SetupSet(r => r.Description = command.Description);
                                                                                        });
                                      mockCommand = MockCommand<ChangeDelayToSchedulerStatusCommand>
                                              .When(command)
                                              .StubGetById(command.Ids[0], delay.Object);
                                  };

        Because of = () => mockCommand.Original.Execute();

        It should_be_verify = () => delay.VerifyAll();
    }
}