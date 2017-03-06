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
    using Incoding.MvcContrib.MVD;

    #endregion

    public class DefaultDispatcher : IDispatcher
    {
        static readonly List<Func<IMessageInterception>> interceptions = new List<Func<IMessageInterception>>();

        public static void SetInterception(Func<IMessageInterception> create)
        {
            interceptions.Add(create);
        }

        #region Fields

        readonly UnitOfWorkCollection unitOfWorkCollection = new UnitOfWorkCollection();

        #endregion

        #region Nested classes

        internal class UnitOfWorkCollection : Dictionary<MessageExecuteSetting, Lazy<IUnitOfWork>>, IDisposable
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

            public void Commit()
            {
                this.Select(r => r.Value)
                    .DoEach(r =>
                            {
                                if (r.IsValueCreated)
                                    r.Value.Commit();
                            });
            }
        }

        #endregion

        #region IDispatcher Members

        public void Push(CommandComposite composite)
        {
            bool isOuterCycle = !unitOfWorkCollection.Any();
            var isFlush = composite.Parts.Any(s => s is CommandBase);
            try
            {
                foreach (var groupMessage in composite.Parts.GroupBy(part => part.Setting, r => r))
                {
                    foreach (var part in groupMessage)
                    {
                        if (isOuterCycle)
                        {
                            if(part.Setting.UID == Guid.Empty)
                            part.Setting.UID = Guid.NewGuid();
                            part.Setting.IsOuter = true;
                        }
                        var unitOfWork = unitOfWorkCollection.AddOrGet(groupMessage.Key, isFlush);
                        foreach (var interception in interceptions)
                            interception().OnBefore(part);

                        part.OnExecute(this, unitOfWork);

                        foreach (var interception in interceptions)
                            interception().OnAfter(part);

                        var isFlushInIteration = part is CommandBase;
                        if (unitOfWork.IsValueCreated && isFlushInIteration)
                            unitOfWork.Value.Flush();
                    }
                }
                if (isOuterCycle && isFlush)
                    this.unitOfWorkCollection.Commit();
            }
            finally
            {
                if (isOuterCycle)
                    unitOfWorkCollection.Dispose();
            }
        }

        public TResult Query<TResult>(QueryBase<TResult> message, MessageExecuteSetting executeSetting = null)
        {
            Push(new CommandComposite(message, executeSetting));
            return (TResult)message.Result;
        }

        #endregion
    }
}