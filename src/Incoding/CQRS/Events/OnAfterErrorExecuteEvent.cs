namespace Incoding.CQRS
{
    #region << Using >>

    using System;

    #endregion

    public class OnAfterErrorExecuteEvent : DispatcherEventBase
    {
        #region Fields

        readonly Exception exception;

        #endregion

        #region Constructors

        public OnAfterErrorExecuteEvent(object message, Exception exception)
                : base(message)
        {
            this.exception = exception;
        }

        #endregion

        #region Properties

        public Exception Exception
        {
            get { return this.exception; }
        }

        #endregion
    }
}