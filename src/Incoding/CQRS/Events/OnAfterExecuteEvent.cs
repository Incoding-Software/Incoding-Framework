namespace Incoding.CQRS
{
    public class OnAfterExecuteEvent : DispatcherEventBase
    {
        #region Constructors

        public OnAfterExecuteEvent(object message)
                : base(message) { }

        #endregion
    }
}