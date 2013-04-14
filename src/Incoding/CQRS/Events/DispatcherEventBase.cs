namespace Incoding.CQRS
{
    #region << Using >>

    using Incoding.EventBroker;

    #endregion

    public abstract class DispatcherEventBase : IEvent
    {
        // ReSharper disable NotAccessedField.Local
        #region Fields

        readonly object message;

        #endregion

        // ReSharper restore NotAccessedField.Local
        #region Constructors

        protected DispatcherEventBase(object message)
        {
            this.message = message;
        }

        #endregion
    }
}