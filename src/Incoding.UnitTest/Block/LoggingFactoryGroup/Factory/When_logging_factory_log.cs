namespace Incoding.UnitTest.Block
{
    #region << Using >>

    using System;
    using Incoding.Block.Logging;
    using Machine.Specifications;using Incoding.MSpecContrib;

    #endregion

    [Subject(typeof(LoggingFactory))]
    public class When_logging_factory_log : Context_logging_factory
    {
        Establish establish = () => loggingFactory.Initialize(logging => logging.WithPolicy(LoggingPolicy.For(LogType.Debug).Use(defaultMockLogger.Object)));

        Because of = () => loggingFactory.Log(LogType.Debug, Pleasure.Generator.TheSameString(), Pleasure.Generator.Invent<ArgumentException>(), Pleasure.Generator.The20120406Noon());

        It should_be_log = () =>
                               {
                                   Action<LogMessage> verify = message =>
                                                                   {
                                                                       message.Message.ShouldEqual(Pleasure.Generator.TheSameString());
                                                                       message.Exception.ShouldBeOfType<ArgumentException>();
                                                                       message.State.ShouldEqual(Pleasure.Generator.The20120406Noon());
                                                                   };
                                   defaultMockLogger.Verify(r => r.Log(Pleasure.MockIt.Is(verify)));
                               };
    }
}