namespace Incoding.UnitTest.Block
{
    #region << Using >>

    using System.Diagnostics;
    using Incoding.Block.Logging;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(EventLogger))]
    public class When_event_logger_log : Context_Logger
    {
        #region Estabilish value

        const string testLog = "Test Log";

        static string sourceName = "Incoding Test";

        #endregion

        Establish establish = () =>
                                  {
                                      if (EventLog.Exists(sourceName))
                                          EventLog.DeleteEventSource(sourceName);

                                      logger = new EventLogger(sourceName, testLog, EventLogEntryType.Information);
                                  };

        Because of = () => logger.Log(new LogMessage(Pleasure.Generator.String(), null, null));

        It should_be_exists_event = () => EventLog.Exists(testLog).ShouldBeTrue();

        It should_be_exists_source = () => EventLog.SourceExists(sourceName).ShouldBeTrue();
    }
}