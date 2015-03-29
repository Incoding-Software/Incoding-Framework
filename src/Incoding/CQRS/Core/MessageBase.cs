namespace Incoding.CQRS
{
    #region << Using >>

    using System;
    using System.Runtime.Serialization;
    using Incoding.Block.IoC;
    using Incoding.Data;
    using Incoding.EventBroker;
    using Incoding.Maybe;
    using Incoding.Quality;
    using Newtonsoft.Json;

    #endregion

    public abstract class MessageBase<TResult> : IMessage<TResult>
    {
        #region Constructors

        protected MessageBase()
        {
            Result = default(TResult);
            this.lazyRepository = new Lazy<IRepository>(() =>
                                                        {
                                                            Setting.With(r => r.unitOfWork).Do(work => work.Open());
                                                            IRepository repository = string.IsNullOrWhiteSpace(Setting.With(r => r.DataBaseInstance))
                                                                                             ? IoCFactory.Instance.TryResolve<IRepository>()
                                                                                             : IoCFactory.Instance.TryResolveByNamed<IRepository>(Setting.DataBaseInstance);
                                                            // we are sure that UnitOfWork will open corresponding Session object
                                                            repository.SetProvider(Setting.With(r => r.unitOfWork).GetSession());
                                                            return repository;
                                                        });
            this.lazyEventBroker = new Lazy<IEventBroker>(() => IoCFactory.Instance.TryResolve<IEventBroker>());
            this.messageDispatcher = new Lazy<MessageDispatcher>(() => new MessageDispatcher(Setting.With(r => r.outerDispatcher), Setting));
        }

        #endregion

        #region Nested classes

        protected class MessageDispatcher
        {
            #region Constructors

            public MessageDispatcher(IDispatcher dispatcher, MessageExecuteSetting setting)
            {
                if (dispatcher == null)
                    throw new Exception("External dispatcher should not be null on internal dispatcher creation");
                this.dispatcher = dispatcher;
                this.outerSetting = setting;
            }

            #endregion

            #region Fields

            readonly IDispatcher dispatcher;

            readonly MessageExecuteSetting outerSetting;

            #endregion

            #region Api Methods

            public TQueryResult Query<TQueryResult>(QueryBase<TQueryResult> query, Action<MessageExecuteSetting> configuration = null) where TQueryResult : class
            {
                var setting = new MessageExecuteSetting
                              {
                                      Connection = outerSetting.Connection,
                                      DataBaseInstance = outerSetting.DataBaseInstance
                              };
                configuration.Do(action => action(setting));
                return this.dispatcher.Query(query, setting);
            }

            public void Push(CommandBase command, Action<MessageExecuteSetting> configuration = null)
            {
                var setting = new MessageExecuteSetting
                              {
                                      Connection = outerSetting.Connection,
                                      DataBaseInstance = outerSetting.DataBaseInstance
                              };
                configuration.Do(action => action(setting));
                this.dispatcher.Push(command, new MessageExecuteSetting());
            }

            public void Delay(CommandBase command, Action<MessageDelaySetting> configuration = null)
            {
                command.Setting = command.Setting ?? new MessageExecuteSetting
                                                     {
                                                             Connection = outerSetting.Connection,
                                                             DataBaseInstance = outerSetting.DataBaseInstance
                                                     };
                this.dispatcher.Delay(command, configuration);
            }

            #endregion
        }

        #endregion

        #region Fields

        readonly Lazy<IRepository> lazyRepository;

        readonly Lazy<IEventBroker> lazyEventBroker;

        readonly Lazy<MessageDispatcher> messageDispatcher;

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

        [IgnoreCompare("Design fixed"), JsonIgnore, IgnoreDataMember]
        public virtual TResult Result { get; protected set; }

        [IgnoreCompare("Design fixed"), IgnoreDataMember]
        public MessageExecuteSetting Setting { get; set; }

        public abstract void Execute();

        #endregion
    }
}