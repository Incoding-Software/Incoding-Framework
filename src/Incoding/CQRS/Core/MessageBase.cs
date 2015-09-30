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

    public abstract class MessageBase<TResult> : IMessage<TResult>
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
        public virtual TResult Result { get; protected set; }

        [IgnoreCompare("Design fixed"), IgnoreDataMember]
        public MessageExecuteSetting Setting { get; set; }

        public virtual void OnExecute(IDispatcher current, IUnitOfWork unitOfWork)
        {
            Result = default(TResult);
            lazyRepository = new Lazy<IRepository>(() => { return unitOfWork.GetRepository(); });
            lazyEventBroker = new Lazy<IEventBroker>(() => IoCFactory.Instance.TryResolve<IEventBroker>());
            messageDispatcher = new Lazy<MessageDispatcher>(() => new MessageDispatcher(current, Setting));
            Execute();
        }

        #endregion

        #region Nested classes

        protected class AsyncMessageDispatcher
        {
            #region Fields

            readonly IDispatcher dispatcher;

            readonly MessageExecuteSetting outerSetting;

            #endregion

            #region Constructors

            public AsyncMessageDispatcher(IDispatcher dispatcher, MessageExecuteSetting setting)
            {
                if (dispatcher == null)
                    throw new Exception("External dispatcher should not be null on internal dispatcher creation");
                this.dispatcher = dispatcher;
                outerSetting = setting;
            }

            #endregion

            #region Api Methods

            public Task<TQueryResult> Query<TQueryResult>(QueryBase<TQueryResult> query, Action<MessageExecuteSetting> configuration = null) where TQueryResult : class
            {
                var setting = new MessageExecuteSetting
                              {
                                      Connection = outerSetting.Connection, 
                                      DataBaseInstance = outerSetting.DataBaseInstance
                              };
                configuration.Do(action => action(setting));
                return Task<TQueryResult>.Factory.StartNew(() => dispatcher.Query(query, setting));
            }

            public Task<object> Push(CommandBase command, Action<MessageExecuteSetting> configuration = null)
            {
                var setting = new MessageExecuteSetting
                              {
                                      Connection = outerSetting.Connection, 
                                      DataBaseInstance = outerSetting.DataBaseInstance
                              };
                configuration.Do(action => action(setting));
                return Task.Factory.StartNew(() =>
                                             {
                                                 dispatcher.Push(command, new MessageExecuteSetting());
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
                if (dispatcher == null)
                    throw new Exception("External dispatcher should not be null on internal dispatcher creation");
                this.dispatcher = dispatcher;
                outerSetting = setting;
            }

            #endregion

            #region Api Methods

            public AsyncMessageDispatcher Async()
            {
                return new AsyncMessageDispatcher(dispatcher, outerSetting);
            }

            public MessageDispatcher New(MessageExecuteSetting settings = null)
            {
                return new MessageDispatcher(IoCFactory.Instance.TryResolve<IDispatcher>(), settings ?? new MessageExecuteSetting());
            }

            public TQueryResult Query<TQueryResult>(QueryBase<TQueryResult> query, Action<MessageExecuteSetting> configuration = null)
            {
                var setting = new MessageExecuteSetting
                              {
                                      Connection = outerSetting.Connection, 
                                      DataBaseInstance = outerSetting.DataBaseInstance
                              };
                configuration.Do(action => action(setting));
                return dispatcher.Query(query, setting);
            }

            public void Push(CommandBase command, Action<MessageExecuteSetting> configuration = null)
            {
                var setting = new MessageExecuteSetting
                              {
                                      Connection = outerSetting.Connection, 
                                      DataBaseInstance = outerSetting.DataBaseInstance
                              };
                configuration.Do(action => action(setting));
                dispatcher.Push(command, new MessageExecuteSetting());
            }

            #endregion
        }

        #endregion

        protected abstract void Execute();
    }
}