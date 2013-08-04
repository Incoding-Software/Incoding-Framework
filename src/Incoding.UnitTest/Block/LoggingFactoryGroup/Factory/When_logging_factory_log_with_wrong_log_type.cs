namespace Incoding.UnitTest.Block
{
    #region << Using >>

    using Incoding.Block.Logging;
    using Incoding.MSpecContrib;
    using Machine.Specifications;
    using Moq;
    using It = Machine.Specifications.It;

    #endregion

    [Subject(typeof(LoggingFactory))]
    public class When_logging_factory_log_with_wrong_log_type : Context_logging_factory
    {
        Establish establish = () => loggingFactory.Initialize(logging => logging.WithPolicy(r => r.For(LogType.Debug).Use(defaultMockLogger.Object)));

        Because of = () => loggingFactory.LogMessage(Pleasure.Generator.String(), Pleasure.Generator.String());

        It should_be_not_log = () => defaultMockLogger.Verify(r => r.Log(Moq.It.IsAny<LogMessage>()), Times.Never());
    }
}