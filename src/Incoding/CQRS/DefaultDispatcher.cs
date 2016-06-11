namespace Incoding.CQRS
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Threading;
    using Incoding.Block.IoC;
    using Incoding.Data;
    using Incoding.Maybe;

    #endregion

    public class MessageInterceptionContext
    {
        public IMessage Message { get; set; }

        public DefaultDispatcher Dispatcher { get; set; }
    }

    public interface IMessageInterception
    {
        bool IsSatisfied(MessageInterceptionContext context);

        void OnBefore(MessageInterceptionContext context);

        void OnSuccess(MessageInterceptionContext context);

        void OnError(MessageInterceptionContext context, Exception exception);

        void OnComplete(MessageInterceptionContext context);
    }

    public class DefaultDispatcher : IDispatcher
    {
        public static void SetInterception(params IMessageInterception[] interceptions)
        {
            messageInterceptions.Clear();
            messageInterceptions.AddRange(interceptions);
        }

        #region Nested classes

        public class UnitOfWorkCollection : Dictionary<MessageExecuteSetting, Lazy<IUnitOfWork>>, IDisposable
        {
            #region Disposable

            public void Dispose()
            {
                this.Select(r => r.Value)
                    .DoEach(r =>
                            {
                                if (r.IsValueCreated)
                                    r.Value.Dispose();
                            });
                Clear();
            }

            #endregion

            #region Api Methods

            public Lazy<IUnitOfWork> AddOrGet(MessageExecuteSetting setting, bool isFlush)
            {
                if (!ContainsKey(setting))
                {
                    Add(setting, new Lazy<IUnitOfWork>(() =>
                                                       {
                                                           var unitOfWorkFactory = string.IsNullOrWhiteSpace(setting.DataBaseInstance)
                                                                                           ? IoCFactory.Instance.TryResolve<IUnitOfWorkFactory>()
                                                                                           : IoCFactory.Instance.TryResolveByNamed<IUnitOfWorkFactory>(setting.DataBaseInstance);

                                                           var isoLevel = setting.IsolationLevel.GetValueOrDefault(isFlush ? IsolationLevel.ReadCommitted : IsolationLevel.ReadUncommitted);
                                                           return unitOfWorkFactory.Create(isoLevel, isFlush, setting.Connection);
                                                       }, LazyThreadSafetyMode.None));
                }

                return this[setting];
            }

            #endregion
        }

        #endregion

        #region Fields

        readonly static List<IMessageInterception> messageInterceptions = new List<IMessageInterception>();

        readonly UnitOfWorkCollection unitOfWorkCollection = new UnitOfWorkCollection();

        #endregion

        #region IDispatcher Members

        public void Push(CommandComposite composite)
        {
            bool isOuterCycle = !unitOfWorkCollection.Any();

            foreach (var groupMessage in composite.Parts.GroupBy(part => part.Setting, r => r))
            {
                bool isFlush = groupMessage.Any(r => r is CommandBase);
                foreach (var part in groupMessage)
                {
                    var messageInterceptionContext = new MessageInterceptionContext()
                                                     {
                                                             Dispatcher = this,
                                                             Message = part
                                                     };
                    var allSatisfiedInterceptions = messageInterceptions.Where(r => r.IsSatisfied(messageInterceptionContext)).ToList();
                    bool isThrow = false;

                    try
                    {
                        allSatisfiedInterceptions.DoEach(interception => interception.OnBefore(messageInterceptionContext));
                        var unitOfWork = unitOfWorkCollection.AddOrGet(groupMessage.Key, isFlush);
                        part.OnExecute(this, unitOfWork);
                        allSatisfiedInterceptions.DoEach(interception => interception.OnSuccess(messageInterceptionContext));
                        if (unitOfWork.IsValueCreated)
                            unitOfWork.Value.Flush();
                    }
                    catch (Exception ex)
                    {
                        isThrow = true;
                        allSatisfiedInterceptions.DoEach(interception => interception.OnError(messageInterceptionContext, ex));
                        throw;
                    }
                    finally
                    {
                        allSatisfiedInterceptions.DoEach(interception => interception.OnComplete(messageInterceptionContext));
                        if (isThrow && isOuterCycle)
                            unitOfWorkCollection.Dispose();
                    }
                }
            }

            if (isOuterCycle)
                unitOfWorkCollection.Dispose();
        }

        public TResult Query<TResult>(QueryBase<TResult> message, MessageExecuteSetting executeSetting = null)
        {
            var commandComposite = new CommandComposite();
            commandComposite.Quote(message, executeSetting);
            Push(commandComposite);
            return (TResult)message.Result;
        }

        #endregion
    }
}