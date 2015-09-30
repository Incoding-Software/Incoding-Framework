namespace Incoding.CQRS
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Incoding.Block.IoC;
    using Incoding.Data;
    using Incoding.EventBroker;
    using Incoding.Maybe;

    #endregion

    public class DefaultDispatcher : IDispatcher
    {
        #region Fields

        readonly Dictionary<MessageExecuteSetting, IUnitOfWork> unitOfWorkCollection = new Dictionary<MessageExecuteSetting, IUnitOfWork>();

        #endregion

        #region IDispatcher Members

        public void Push(CommandComposite composite)
        {
            bool isOuterCycle = !this.unitOfWorkCollection.Any();
            var eventBroker = IoCFactory.Instance.TryResolve<IEventBroker>() ?? new DefaultEventBroker();

            foreach (var groupMessage in composite.Parts.GroupBy(part => part.Setting, r => r))
            {
                var messages = groupMessage.ToList();
                bool isFlush = messages.Any(r => r is CommandBase);
                var sessing = groupMessage.Key;

                if (!this.unitOfWorkCollection.ContainsKey(sessing))
                {
                    var unitOfWorkFactory = string.IsNullOrWhiteSpace(sessing.DataBaseInstance)
                                                    ? IoCFactory.Instance.TryResolve<IUnitOfWorkFactory>()
                                                    : IoCFactory.Instance.TryResolveByNamed<IUnitOfWorkFactory>(sessing.DataBaseInstance);
                    var isoLevel = sessing.IsolationLevel.GetValueOrDefault(isFlush ? IsolationLevel.ReadCommitted : IsolationLevel.ReadUncommitted);
                    this.unitOfWorkCollection.Add(sessing, unitOfWorkFactory.Create(isoLevel, sessing.Connection));
                }
                IUnitOfWork unitOfWork = this.unitOfWorkCollection[sessing];

                foreach (var part in messages)
                {
                    bool isThrow = false;
                    try
                    {
                        eventBroker.Publish(new OnBeforeExecuteEvent(part));
                        part.OnExecute(this, unitOfWork);
                        eventBroker.Publish(new OnAfterExecuteEvent(part));

                        if (isFlush)
                            unitOfWork.Flush();
                    }
                    catch (Exception exception)
                    {
                        isThrow = true;

                        var onAfterErrorExecuteEvent = new OnAfterErrorExecuteEvent(part, exception);
                        if (eventBroker.HasSubscriber(onAfterErrorExecuteEvent))
                            eventBroker.Publish(onAfterErrorExecuteEvent);

                        throw;
                    }
                    finally
                    {
                        eventBroker.Publish(new OnCompleteExecuteEvent(part));
                        if (isThrow && isOuterCycle)
                        {
                            this.unitOfWorkCollection.Select(r => r.Value)
                                .DoEach(r => r.Dispose());
                            this.unitOfWorkCollection.Clear();
                        }
                    }
                }
            }

            if (isOuterCycle)
            {
                this.unitOfWorkCollection.Select(r => r.Value)
                    .DoEach(r =>
                            {
                                r.Commit();
                                r.Dispose();
                            });
                this.unitOfWorkCollection.Clear();
            }
        }

        public TResult Query<TResult>(QueryBase<TResult> message, MessageExecuteSetting executeSetting = null)
        {
            var commandComposite = new CommandComposite();
            commandComposite.Quote(message, executeSetting);
            Push(commandComposite);
            return message.Result;
        }

        #endregion
    }
}