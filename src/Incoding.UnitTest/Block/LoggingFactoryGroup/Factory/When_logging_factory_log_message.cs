namespace Incoding.UnitTest.Block
{
    #region << Using >>

    using System;
    using Incoding.Block.Logging;
    using Machine.Specifications;using Incoding.MSpecContrib;
    using Moq;
    using It = Machine.Specifications.It;

    #endregion

    [Subject(typeof(LoggingFactory))]
    public class When_logging_factory_log_message : Context_logging_factory
    {
        Establish establish = () => loggingFactory.Initialize(logging => logging.WithPolicy(LoggingPolicy.For(LogType.Trace).Use(defaultMockLogger.Object)));

        Because of = () => loggingFactory.LogMessage(LogType.Trace, Pleasure.Generator.TheSameString());

        It should_be_log = () =>
                               {
                                   Action<LogMessage> verify = message =>
                                                                   {
                                                                       message.Exception.ShouldBeNull();
                                                                       message.State.ShouldBeNull();
                                                                       message.Message.ShouldEqual(Pleasure.Generator.TheSameString());
                                                                   };
                                   defaultMockLogger.Verify(r => r.Log(Pleasure.MockIt.Is(verify)), Times.Once());
                               };
    }
}