namespace Incoding.CQRS
{
    public class OnBeforeExecuteEvent : DispatcherEventBase
    {
        #region Constructors

        public OnBeforeExecuteEvent(object message)
                : base(message) { }

        #endregion
    }
}