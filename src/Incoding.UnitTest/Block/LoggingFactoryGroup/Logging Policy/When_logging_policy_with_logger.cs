namespace Incoding.UnitTest.Block
{
    #region << Using >>

    using Incoding.Block.Logging;
    using Machine.Specifications;using Incoding.MSpecContrib;
    using Moq;
    using It = Machine.Specifications.It;

    #endregion

    [Subject(typeof(LoggingPolicy))]
    public class When_logging_policy_with_logger : Context_Logging_Policy
    {
        Establish establish = () =>
                                  {
                                      loggingPolicy = LoggingPolicy
                                              .For()
                                              .Use(() => defaultMockLogger.Object);
                                  };

        Because of = () => loggingPolicy.Log(new LogMessage(Pleasure.Generator.String(), null, null));

        It should_be_log = () => defaultMockLogger.Verify(r => r.Log(Moq.It.IsAny<LogMessage>()), Times.Once());
    }
}