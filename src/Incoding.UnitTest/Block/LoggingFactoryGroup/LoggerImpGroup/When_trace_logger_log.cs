namespace Incoding.UnitTest.Block
{
    #region << Using >>

    using System;
    using Incoding.Block.Logging;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(TraceLogger))]
    public class When_trace_logger_log : Context_Logger
    {
        #region Establish value

        static Exception exception;

        #endregion

        Establish establish = () => { logger = new TraceLogger("TraceName"); };

        Because of = () => { exception = Catch.Exception(() => logger.Log(new LogMessage(Pleasure.Generator.String(), null, null))); };

        It should_be_log_without_exception = () => exception.ShouldBeNull();
    }
}