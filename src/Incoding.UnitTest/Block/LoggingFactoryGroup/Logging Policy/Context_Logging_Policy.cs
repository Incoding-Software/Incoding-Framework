namespace Incoding.UnitTest.Block
{
    #region << Using >>

    using Incoding.Block.Logging;
    using Moq;

    #endregion

    public class Context_Logging_Policy
    {
        #region Static Fields

        protected static LoggingPolicy loggingPolicy;

        protected static Mock<ILogger> defaultMockLogger;

        #endregion

        #region Constructors

        protected Context_Logging_Policy()
        {
            defaultMockLogger = new Mock<ILogger>();
        }

        #endregion
    }
}