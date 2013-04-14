namespace Incoding.Block.Logging
{
    #region << Using >>

    using System;

    #endregion

    public class LogMessage
    {
        #region Constructors

        public LogMessage(string message, Exception snapshotException, object state)
        {
            State = state;
            Message = message;
            Exception = snapshotException;
        }

        #endregion

        #region Properties

        public object State { get; private set; }

        public string Message { get; private set; }

        public Exception Exception { get; private set; }

        #endregion
    }
}