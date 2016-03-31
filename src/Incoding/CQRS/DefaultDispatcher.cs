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
        #region Fields

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
                    bool isThrow = false;
                    try
                    {                        
                        var unitOfWork = unitOfWorkCollection.AddOrGet(groupMessage.Key, isFlush);
                        part.OnExecute(this, unitOfWork);
                        if (unitOfWork.IsValueCreated)
                            unitOfWork.Value.Flush();                        
                    }
                    catch
                    {
                        isThrow = true;                        
                        throw;
                    }
                    finally
                    {                        
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

        #region Nested classes

        public class UnitOfWorkCollection : Dictionary<MessageExecuteSetting, Lazy<IUnitOfWork>>, IDisposable
        {
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
        }

        #endregion
    }
}