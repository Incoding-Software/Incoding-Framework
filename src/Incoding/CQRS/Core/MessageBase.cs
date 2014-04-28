namespace Incoding.CQRS
{
    #region << Using >>

    using System;
    using Incoding.Block.IoC;
    using Incoding.Data;
    using Incoding.EventBroker;
    using Incoding.Maybe;
    using Incoding.Quality;
    using Newtonsoft.Json;

    #endregion

    public abstract class MessageBase<TResult> : IMessage<TResult>
    {
        #region Fields

        readonly Lazy<IRepository> lazyRepository;

        readonly Lazy<IEventBroker> lazyEventBroker;

        readonly Lazy<MessageDispatcher> messageDispatcher;

        #endregion

        #region Constructors

        protected MessageBase()
        {
            Result = default(TResult);
            this.lazyRepository = new Lazy<IRepository>(() =>
                                                            {
                                                                Setting.With(r => r.UnitOfWork)
                                                                       .Do(work => work.Open());
                                                                return string.IsNullOrWhiteSpace(Setting.With(r => r.DataBaseInstance))
                                                                               ? IoCFactory.Instance.TryResolve<IRepository>()
                                                                               : IoCFactory.Instance.TryResolveByNamed<IRepository>(Setting.DataBaseInstance);
                                                            });
            this.lazyEventBroker = new Lazy<IEventBroker>(() => IoCFactory.Instance.TryResolve<IEventBroker>());
            this.messageDispatcher = new Lazy<MessageDispatcher>(() => new MessageDispatcher(Setting.With(r => r.UnitOfWork)));
        }

        #endregion

        #region Properties

        [IgnoreCompare("System")]
        protected IRepository Repository { get { return this.lazyRepository.Value; } }

        [IgnoreCompare("System")]
        protected MessageDispatcher Dispatcher { get { return this.messageDispatcher.Value; } }

        [IgnoreCompare("System"), Obsolete("Please use Dispatcher.Push or Dispatcher.Query instead events")]
        protected IEventBroker EventBroker { get { return this.lazyEventBroker.Value; } }

        #endregion

        #region IMessage<TResult> Members

        [IgnoreCompare("Design fixed"), JsonIgnore]
        public virtual TResult Result { get; protected set; }

        [IgnoreCompare("Design fixed")]
        public MessageExecuteSetting Setting { get; set; }

        public abstract void Execute();

        #endregion

        #region Nested classes

        protected class MessageDispatcher
        {
            #region Fields

            readonly IUnitOfWork unitOfWork;

            readonly IDispatcher dispatcher;

            #endregion

            #region Constructors

            public MessageDispatcher(IUnitOfWork unitOfWork)
            {
                this.unitOfWork = unitOfWork;
                this.dispatcher = IoCFactory.Instance.TryResolve<IDispatcher>();
            }

            #endregion

            #region Api Methods

            public TQueryResult Query<TQueryResult>(QueryBase<TQueryResult> query, Action<MessageExecuteSetting> configuration = null) where TQueryResult : class
            {
                var setting = new MessageExecuteSetting
                                  {
                                          UnitOfWork = this.unitOfWork
                                  };
                configuration.Do(action => action(setting));
                return this.dispatcher.Query(query, setting);
            }

            public void Push(CommandBase command, Action<MessageExecuteSetting> configuration = null)
            {
                var setting = new MessageExecuteSetting();
                configuration.Do(action => action(setting));
                this.dispatcher.Push(command, new MessageExecuteSetting
                                                  {
                                                          UnitOfWork = this.unitOfWork,
                                                          Delay = setting.Delay
                                                  });
            }

            public void Delay(CommandBase command, Action<MessageDelaySetting> configuration = null)
            {
                this.dispatcher.Delay(command, configuration);
            }

            #endregion
        }

        #endregion
    }
}