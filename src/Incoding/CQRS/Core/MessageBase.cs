namespace Incoding.CQRS
{
    #region << Using >>

    using System;
    using System.Runtime.Serialization;
    using System.Threading.Tasks;
    using Incoding.Block.IoC;
    using Incoding.Data;
    using Incoding.EventBroker;
    using Incoding.Maybe;
    using Incoding.Quality;
    using Newtonsoft.Json;

    #endregion

    public abstract class MessageBase : IMessage
    {
        #region Fields

        Lazy<IRepository> lazyRepository;

        Lazy<IEventBroker> lazyEventBroker;

        Lazy<MessageDispatcher> messageDispatcher;

        #endregion

        #region Properties

        [IgnoreCompare("System"), JsonIgnore]
        protected IRepository Repository { get { return lazyRepository.Value; } }

        [IgnoreCompare("System"), JsonIgnore]
        protected MessageDispatcher Dispatcher { get { return messageDispatcher.Value; } }

        [IgnoreCompare("System"), Obsolete("Please use Dispatcher.Push or Dispatcher.Query instead events"), JsonIgnore]
        protected IEventBroker EventBroker { get { return lazyEventBroker.Value; } }

        #endregion

        #region IMessage<TResult> Members

        [IgnoreCompare("Design fixed"), JsonIgnore, IgnoreDataMember]
        public virtual object Result { get; protected set; }

        [IgnoreCompare("Design fixed"), IgnoreDataMember]
        public MessageExecuteSetting Setting { get; set; }

        public virtual void OnExecute(IDispatcher current, Lazy<IUnitOfWork> unitOfWork)
        {
            Result = null;
            lazyRepository = new Lazy<IRepository>(() => unitOfWork.Value.GetRepository());
            lazyEventBroker = new Lazy<IEventBroker>(() => IoCFactory.Instance.TryResolve<IEventBroker>());
            messageDispatcher = new Lazy<MessageDispatcher>(() => new MessageDispatcher(current, Setting));
            Execute();
        }

        #endregion

        #region Api Methods

        protected abstract void Execute();

        #endregion

        #region Nested classes

        protected class AsyncMessageDispatcher
        {
            #region Fields

            readonly MessageDispatcher dispatcher;

            #endregion

            #region Constructors

            public AsyncMessageDispatcher(MessageDispatcher dispatcher)
            {
                this.dispatcher = dispatcher;
            }

            #endregion

            #region Api Methods

            public Task<TQueryResult> Query<TQueryResult>(QueryBase<TQueryResult> query, Action<MessageExecuteSetting> configuration = null) where TQueryResult : class
            {
                return Task<TQueryResult>.Factory.StartNew(() => dispatcher.Query(query, configuration));
            }

            public Task<object> Push(CommandBase command, Action<MessageExecuteSetting> configuration = null)
            {
                return Task.Factory.StartNew(() =>
                                             {
                                                 dispatcher.Push(command, configuration);
                                                 return command.Result;
                                             });
            }

            #endregion
        }

        protected class MessageDispatcher
        {
            #region Fields

            readonly IDispatcher dispatcher;

            readonly MessageExecuteSetting outerSetting;

            #endregion

            #region Constructors

            public MessageDispatcher(IDispatcher dispatcher, MessageExecuteSetting setting)
            {
                Guard.NotNull("dispatcher", dispatcher, errorMessage: "External dispatcher should not be null on internal dispatcher creation");
                this.dispatcher = dispatcher;
                outerSetting = setting;
            }

            #endregion

            #region Api Methods

            public AsyncMessageDispatcher Async()
            {
                return new AsyncMessageDispatcher(this);
            }

            public IDispatcher New()
            {
                return IoCFactory.Instance.TryResolve<IDispatcher>();
            }

            public TQueryResult Query<TQueryResult>(QueryBase<TQueryResult> query, Action<MessageExecuteSetting> configuration = null)
            {
                configuration.Do(action => action(outerSetting));
                return dispatcher.Query(query, outerSetting);
            }

            public void Push(CommandBase command, Action<MessageExecuteSetting> configuration = null)
            {
                configuration.Do(action => action(outerSetting));
                dispatcher.Push(command, outerSetting);
            }

            #endregion
        }

        #endregion

    }
}