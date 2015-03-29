using System.Security;

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

            ////ncrunch: no coverage start
            if (!EventLog.SourceExists(sourceName))
                EventLog.CreateEventSource(sourceName, logName);
            ////ncrunch: no coverage end
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