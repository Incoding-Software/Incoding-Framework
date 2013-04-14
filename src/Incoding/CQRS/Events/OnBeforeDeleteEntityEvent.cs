namespace Incoding.CQRS
{
    #region << Using >>

    using Incoding.Data;
    using Incoding.EventBroker;

    #endregion

    public class OnBeforeDeleteEntityEvent : IEvent
    {
        #region Fields

        readonly IEntity entity;

        #endregion

        #region Constructors

        public OnBeforeDeleteEntityEvent(IEntity entity)
        {
            this.entity = entity;
        }

        #endregion

        #region Properties

        public IEntity Entity
        {
            get { return this.entity; }
        }

        #endregion
    }
}