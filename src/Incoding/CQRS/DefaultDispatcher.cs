using System.Windows.Forms;

namespace Incoding.CQRS
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Incoding.Block;
    using Incoding.Block.IoC;
    using Incoding.Data;
    using Incoding.EventBroker;
    using Incoding.Maybe;

    #endregion

    public class DefaultDispatcher : IDispatcher
    {
        private class UnitOfWorkItem
        {
            public IUnitOfWork UnitOfWork { get; set; }
            public bool IsFlush { get; set; }
        }

        private Dictionary<int, UnitOfWorkItem> unitOfWorkCollection = null;
        
        #region IDispatcher Members

        public void Push(CommandComposite composite)
        {
            bool isOuterCycle = false;
            if (unitOfWorkCollection == null)
            {
                isOuterCycle = true;
                unitOfWorkCollection = new Dictionary<int, UnitOfWorkItem>();
            }
            //var delays = new List<IMessage<object>>();
            var eventBroker = IoCFactory.Instance.TryResolve<IEventBroker>();
            //var units = new Dictionary<int, Tuple<IUnitOfWork, bool>>();

            foreach (IGrouping<MessageExecuteSetting, IMessage<object>> groupMessage in composite.Parts.GroupBy(part => part.Setting, r => r))
            {
                var groupMessageKey = groupMessage.Key;
                List<IMessage<object>> messages = groupMessage.ToList();

                bool isFlush = messages.Any(r => r is CommandBase);
                IUnitOfWork unitOfWork;

                var unitOfWorkKey = groupMessageKey.GetHashCode();
                if (unitOfWorkCollection.ContainsKey(unitOfWorkKey))
                {
                    unitOfWork = unitOfWorkCollection[unitOfWorkKey].UnitOfWork;
                }
                else 
                {
                    var unitOfWorkFactory = string.IsNullOrWhiteSpace(groupMessageKey.DataBaseInstance)
                                                    ? IoCFactory.Instance.TryResolve<IUnitOfWorkFactory>()
                                                    : IoCFactory.Instance.TryResolveByNamed<IUnitOfWorkFactory>(groupMessageKey.DataBaseInstance);
                    unitOfWork = unitOfWorkFactory.Create(isFlush ? IsolationLevel.ReadCommitted : IsolationLevel.ReadUncommitted, groupMessageKey.Connection);
                    unitOfWorkCollection.Add(groupMessageKey.GetHashCode(), new UnitOfWorkItem
                    {
                        UnitOfWork = unitOfWork,
                        IsFlush = isFlush
                    });
                }
                
                foreach (IMessage<object> part in messages)
                {
                    bool isThrow = false;
                    try
                    {
                        part.Setting.outerDispatcher = this;
                        part.Setting.unitOfWork = unitOfWork;
                        part.Setting.OnBefore.Do(action => action(part));
                        if (!part.Setting.Mute.HasFlag(MuteEvent.OnBefore))
                            eventBroker.Publish(new OnBeforeExecuteEvent(part));

                        part.Execute();

                        part.Setting.OnAfter.Do(action => action(part));
                        if (!part.Setting.Mute.HasFlag(MuteEvent.OnAfter))
                            eventBroker.Publish(new OnAfterExecuteEvent(part));

                        if (isFlush)
                            unitOfWork.Flush();
                    }
                    catch (Exception exception)
                    {
                        isThrow = true;
                        part.Setting.OnError.Do(action => action(part, exception));
                        var onAfterErrorExecuteEvent = new OnAfterErrorExecuteEvent(part, exception);
                        if (!part.Setting.Mute.HasFlag(MuteEvent.OnError) && eventBroker.HasSubscriber(onAfterErrorExecuteEvent))
                            eventBroker.Publish(onAfterErrorExecuteEvent);

                        throw;
                    }
                    finally
                    {
                        part.Setting.OnComplete.Do(action => action(part));
                        if (!part.Setting.Mute.HasFlag(MuteEvent.OnComplete))
                            eventBroker.Publish(new OnCompleteExecuteEvent(part));
                        if (isThrow)
                        {
                            if (isOuterCycle)
                            {
                                unitOfWorkCollection.Select(r => r.Value)
                                    .DoEach(r => r.UnitOfWork.Dispose());
                                unitOfWorkCollection = null;
                            }
                        }
                    }
                }
            }

            if (isOuterCycle)
            {
                unitOfWorkCollection.Select(r => r.Value)
                    .DoEach(r =>
                    {
                        if (r.IsFlush)
                            r.UnitOfWork.Commit();

                        r.UnitOfWork.Dispose();
                    });
                unitOfWorkCollection = null;
            }
            //foreach (var messages in delays.GroupBy(r => r.Setting.Delay, r => r))
            //{
            //    IoCFactory.Instance.TryResolve<IDispatcher>()
            //              .Push(new AddDelayToSchedulerCommand
            //                        {
            //                                Commands = messages
            //                                        .ToList()
            //                        }, new MessageExecuteSetting
            //                               {
            //                                       Connection = messages.Key.Connection,
            //                                       DataBaseInstance = messages.Key.DataBaseInstance
            //                               });
            //}
        }

        public TResult Query<TResult>(QueryBase<TResult> message, MessageExecuteSetting executeSetting = null) where TResult : class
        {
            var commandComposite = new CommandComposite();
            commandComposite.Quote(message, executeSetting);
            Push(commandComposite);
            return message.Result;
        }

        #endregion
    }
}