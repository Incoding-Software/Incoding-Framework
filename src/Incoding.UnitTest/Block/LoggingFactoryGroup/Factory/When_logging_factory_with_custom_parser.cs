namespace Incoding.UnitTest.Block
{
    #region << Using >>

    using System;
    using Incoding.Block.Logging;
    using Machine.Specifications;
    using Moq;
    using It = Machine.Specifications.It;

    #endregion

    [Subject(typeof(LoggingFactory))]
    public class When_logging_factory_with_custom_parser : Context_logging_factory
    {
        #region Establish value

        static Mock<IParserException> mockParse;

        #endregion

        Establish establish = () =>
                                  {
                                      mockParse = new Mock<IParserException>();
                                      loggingFactory.Initialize(logging => logging.WithParser(mockParse.Object));
                                  };

        Because of = () => loggingFactory.LogException(LogType.Trace, new ArgumentException());

        It should_be_parse = () => mockParse.Verify(r => r.Parse(Moq.It.IsAny<ArgumentException>()), Times.Once());
    }
}