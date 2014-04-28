namespace Incoding.UnitTest.Block
{
    #region << Using >>

    using Incoding.Block.Logging;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(ActionLogger))]
    public class When_lambda_logger_log : Context_Logger
    {
        #region Establish value

        static bool isLog;

        #endregion

        Establish establish = () => { logger = new ActionLogger(logMessage => { isLog = true; }); };

        Because of = () => logger.Log(new LogMessage(Pleasure.Generator.String(), null, null));

        It should_be_log = () => isLog.ShouldBeTrue();
    }
}