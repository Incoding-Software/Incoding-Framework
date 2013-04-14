namespace Incoding.CQRS
{
    public class OnCompleteExecuteEvent : DispatcherEventBase
    {
        #region Constructors

        public OnCompleteExecuteEvent(object message)
                : base(message) { }

        #endregion
    }
}