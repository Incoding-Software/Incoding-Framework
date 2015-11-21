namespace Incoding.UnitTest.Block
{
    #region << Using >>

    using System;
    using Incoding.Block.Logging;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(LoggingFactory))]
    public class When_logging_factory_log_exception : Context_logging_factory
    {
        Establish establish = () => loggingFactory.Initialize(logging =>
                                                                  {
                                                                      var mockParse = Pleasure.MockAsObject<IParserException>(mock => mock.Setup(r => r.Parse(Pleasure.MockIt.IsAny<ArgumentException>())).Returns(Pleasure.Generator.TheSameString));
                                                                      logging.WithParser(mockParse);
                                                                      logging.WithPolicy(r=>r.For(LogType.Debug).Use(defaultMockLogger.Object));
                                                                  });

        Because of = () => loggingFactory.LogException(LogType.Debug, new ArgumentException());

        It should_be_log = () =>
                               {
                                   Action<LogMessage> verify = message =>
                                                                   {
                                                                       message.Exception.ShouldBeAssignableTo<ArgumentException>();
                                                                       message.Message.ShouldEqual(Pleasure.Generator.TheSameString());
                                                                       message.State.ShouldBeNull();
                                                                   };
                                   defaultMockLogger.Verify(r => r.Log(Pleasure.MockIt.Is(verify)));
                               };
    }
}