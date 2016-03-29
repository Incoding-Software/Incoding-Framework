namespace Incoding.UnitTest
{
    #region << Using >>

    using System;
    using Incoding.Block;
    using Incoding.Block.Logging;
    using Incoding.MSpecContrib;
    using Machine.Specifications;
    using Moq;
    using It = Machine.Specifications.It;

    #endregion

    [Subject(typeof(StartSchedulerCommand))]
    public class When_start_scheduler_with_global_throw
    {
        Establish establish = () =>
                              {
                                  var command = Pleasure.Generator.Invent<StartSchedulerCommand>(dsl => dsl.Tuning(r => r.DelayToStart, null)
                                                                                                           .Tuning(r => r.Conditional, () => { throw new ArgumentException(); }));
                                  logger = Pleasure.Mock<ILogger>();
                                  LoggingFactory.Instance.Initialize(logging => logging.WithPolicy(policy => policy.For(command.Log_Debug).Use(logger.Object)));

                                  mockMessage = MockCommand<StartSchedulerCommand>
                                          .When(command);
                              };

        Because of = () =>
                     {
                         exception = Catch.Exception(() => mockMessage.Execute());
                         Pleasure.Sleep1000Milliseconds();
                     };

        It should_be_log_once = () => logger.Verify(r => r.Log(Pleasure.MockIt.IsStrong(new LogMessage(string.Empty, new ArgumentException(), null), dsl => dsl.ForwardToAction(message => message.Exception, message => message.Exception.ShouldNotBeNull())
                                                                                                                                                               .ForwardToAction(message => message.Message, message => message.Message.ShouldNotBeEmpty()))), Times.Exactly(2));

        It should_not_be_exception = () => exception.ShouldBeNull();

        #region Establish value

        static Mock<ILogger> logger;

        static Exception exception;

        static MockCommand<StartSchedulerCommand> mockMessage;

        #endregion
    }
}