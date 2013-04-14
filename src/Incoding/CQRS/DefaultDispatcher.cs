namespace Incoding.CQRS
{
    #region << Using >>

    using System;
    using System.Data;
    using System.Linq;
    using Incoding.Block.IoC;
    using Incoding.Data;
    using Incoding.EventBroker;
    using Incoding.Extensions;

    #endregion

    public class DefaultDispatcher : IDispatcher
    {
        #region IDispatcher Members

        public void Push(CommandComposite composite)
        {            
            bool hasAtLeastCommand = composite.Parts.Any(r => r.Message is CommandBase);
            var isolationLevel = hasAtLeastCommand ? IsolationLevel.ReadCommitted : IsolationLevel.ReadUncommitted;
            composite.Parts.Last().Setting.Commit = hasAtLeastCommand;

            var eventBroker = IoCFactory.Instance.TryResolve<IEventBroker>();

            string named = composite.Parts.Last().Setting.DataBaseInstance;
            var connectionString = composite.Parts.Last().Setting.Connection;
            var unitOfWorkFactory = string.IsNullOrWhiteSpace(named)
                                            ? IoCFactory.Instance.TryResolve<IUnitOfWorkFactory>()
                                            : IoCFactory.Instance.TryResolveByNamed<IUnitOfWorkFactory>(named);
            using (var unitOfWork = unitOfWorkFactory.Create(isolationLevel, connectionString))
            {
                foreach (var part in composite.Parts)
                {
                    try
                    {
                        if (!string.IsNullOrWhiteSpace(part.Setting.DataBaseInstance))
                            part.Message.SetValue("dataBaseInstance", part.Setting.DataBaseInstance);

                        part.Setting.OnBefore();
                        if (part.Setting.PublishEventOnBefore)
                            eventBroker.Publish(new OnBeforeExecuteEvent(part.Message));

                        part.Message.Execute();

                        part.Setting.OnAfter();
                        if (part.Setting.PublishEventOnAfter)
                            eventBroker.Publish(new OnAfterExecuteEvent(part.Message));

                        if (part.Setting.Commit)
                            unitOfWork.Commit();
                    }
                    catch (Exception exception)
                    {
                        part.Setting.OnError(exception);
                        OnAfterErrorExecuteEvent onAfterErrorExecuteEvent = new OnAfterErrorExecuteEvent(part.Message, exception);

                        if (part.Setting.PublishEventOnError && eventBroker.HasSubscriber(onAfterErrorExecuteEvent))
                            eventBroker.Publish(onAfterErrorExecuteEvent);
                        else
                        {
                            throw;
                        }
                    }
                    finally
                    {
                        part.Setting.OnComplete();
                        if (part.Setting.PublishEventOnComplete)
                            eventBroker.Publish(new OnCompleteExecuteEvent(part.Message));
                    }
                }
            }
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