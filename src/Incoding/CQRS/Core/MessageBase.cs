namespace Incoding.CQRS
{
    #region << Using >>

    using Incoding.Block.IoC;
    using Incoding.Data;
    using Incoding.EventBroker;
    using Incoding.Quality;

    #endregion

    public abstract class MessageBase<TResult> : IMessage<TResult> where TResult : class
    {
        #region Fields

        readonly string dataBaseInstance = string.Empty;

        #endregion

        #region Constructors

        protected MessageBase()
        {
            Result = default(TResult);
        }

        #endregion

        #region Properties

        [IgnoreCompare("Design fixed")]
        protected IRepository Repository
        {
            get
            {
                return string.IsNullOrWhiteSpace(this.dataBaseInstance)
                               ? IoCFactory.Instance.TryResolve<IRepository>()
                               : IoCFactory.Instance.TryResolveByNamed<IRepository>(this.dataBaseInstance);
            }
        }

        [IgnoreCompare("Design fixed")]
        protected IEventBroker EventBroker
        {
            get { return IoCFactory.Instance.TryResolve<IEventBroker>(); }
        }

        #endregion

        #region IMessage<TResult> Members

        [IgnoreCompare("Design fixed")]
        public virtual TResult Result { get; protected set; }

        public abstract void Execute();

        #endregion
    }
}