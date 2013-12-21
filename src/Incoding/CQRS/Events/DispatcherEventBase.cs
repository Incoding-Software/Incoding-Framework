namespace Incoding.CQRS
{
    #region << Using >>

    using Incoding.EventBroker;

    #endregion

    public abstract class DispatcherEventBase : IEvent
    {
        #region Constructors

        protected DispatcherEventBase(object message)
        {
            Message = message;
        }

        #endregion

        #region Properties

        public object Message { get; private set; }

        #endregion
    }
}