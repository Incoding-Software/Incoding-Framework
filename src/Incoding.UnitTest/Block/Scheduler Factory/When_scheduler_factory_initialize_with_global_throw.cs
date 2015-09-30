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

    [Subject(typeof(SchedulerFactory))]
    public class When_scheduler_factory_initialize_with_global_throw
    {
        #region Establish value

        static Mock<ILogger> logger;

        static Exception exception;

        #endregion

        Establish establish = () =>
                              {
                                  logger = Pleasure.Mock<ILogger>();
                                  LoggingFactory.Instance.Initialize(logging => logging.WithPolicy(policy => policy.For("Test").Use(logger.Object)));
                              };

        Because of = () =>
                     {
                         exception = Catch.Exception(() => SchedulerFactory.Instance.Initialize(scheduler =>
                                                                                                {
                                                                                                    scheduler.Log_Debug = "Test";
                                                                                                    scheduler.Conditional = () => { throw new ArgumentException(); };
                                                                                                }));
                         Pleasure.Sleep1000Milliseconds();
                     };

        It should_not_be_exception = () => exception.ShouldBeNull();

        It should_be_log_once = () => logger.Verify(r => r.Log(Pleasure.MockIt.IsStrong(new LogMessage(string.Empty, new ArgumentException(), null), dsl => dsl.ForwardToAction(message => message.Exception, message => message.Exception.ShouldNotBeNull())
                                                                                                                                                               .ForwardToAction(message => message.Message, message => message.Message.ShouldNotBeEmpty()))), Times.Once());
    }
}