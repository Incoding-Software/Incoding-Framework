namespace Incoding.Block.Logging
{
    #region << Using >>

    using System;

    #endregion

    /// <summary>
    ///     Imp <see cref="ILogger" /> for action
    /// </summary>
    public class ActionLogger : LoggerBase
    {
        #region Fields

        readonly Action<string> log;

        #endregion

        #region Constructors

        public ActionLogger(Action<string> log)
        {
            this.log = log;
        }

        #endregion

        public override void Log(LogMessage logMessage)
        {
            this.log(this.template(logMessage));
        }
    }
}