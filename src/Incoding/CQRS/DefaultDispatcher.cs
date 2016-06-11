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

    public class DefaultDispatcher : IDispatcher
    {
        public DefaultDispatcher SetInterception(IMessageInterception interception)
        {
            this.messageInterceptions.Add(interception);
            return this;
        }

        public abstract class EmptyInterceptionBase : IMessageInterception
        {
            public virtual bool IsSatisfied(IMessage message)
            {
                return true;
            }

            public virtual void OnBefore(IMessage message) { }

            public virtual void OnSuccess(IMessage message) { }

            public virtual void OnError(IMessage message, Exception exception) { }

            public virtual void OnComplete(IMessage message) { }
        }

        public interface IMessageInterception
        {
            bool IsSatisfied(IMessage message);

            void OnBefore(IMessage message);

            void OnSuccess(IMessage message);

            void OnError(IMessage message, Exception exception);

            void OnComplete(IMessage message);
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

        readonly List<IMessageInterception> messageInterceptions = new List<IMessageInterception>();

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
                    var allSatisfiedInterceptions = this.messageInterceptions.Where(r => r.IsSatisfied(part)).ToList();
                    bool isThrow = false;

                    try
                    {
                        allSatisfiedInterceptions.DoEach(interception => interception.OnBefore(part));
                        var unitOfWork = unitOfWorkCollection.AddOrGet(groupMessage.Key, isFlush);
                        part.OnExecute(this, unitOfWork);
                        allSatisfiedInterceptions.DoEach(interception => interception.OnSuccess(part));
                        if (unitOfWork.IsValueCreated)
                            unitOfWork.Value.Flush();
                    }
                    catch (Exception ex)
                    {
                        isThrow = true;
                        allSatisfiedInterceptions.DoEach(interception => interception.OnError(part, ex));
                        throw;
                    }
                    finally
                    {
                        allSatisfiedInterceptions.DoEach(interception => interception.OnComplete(part));
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