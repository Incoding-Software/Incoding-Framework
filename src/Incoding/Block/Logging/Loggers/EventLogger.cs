namespace Incoding.Block.Logging
{
    #region << Using >>

    using System.Diagnostics;

    #endregion

    public class EventLogger : LoggerBase
    {
        #region Fields

        readonly EventLogEntryType logEntryType;

        readonly string logName;

        readonly string sourceName;

        #endregion

        #region Constructors

        public EventLogger(string sourceName, string logName, EventLogEntryType logEntryType)
        {
            this.sourceName = sourceName;
            this.logName = logName;
            this.logEntryType = logEntryType;

            if (!EventLog.SourceExists(sourceName))
                EventLog.CreateEventSource(sourceName, logName);
        }

        #endregion

        public override void Log(LogMessage logMessage)
        {
            using (var eventLog = new EventLog(this.logName))
            {
                eventLog.Source = this.sourceName;
                eventLog.WriteEntry(this.template(logMessage), this.logEntryType);
            }
        }
    }
}