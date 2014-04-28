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
        #region IDispatcher Members

        public void Push(CommandComposite composite)
        {
            var delays = new List<IMessage<object>>();
            var eventBroker = IoCFactory.Instance.TryResolve<IEventBroker>();
            var units = new Dictionary<int, Tuple<IUnitOfWork, bool>>();

            foreach (var groupMessage in composite.Parts.GroupBy(part => part.Setting, r => r))
            {
                if (groupMessage.Key.Delay != null)
                {
                    delays.AddRange(groupMessage);
                    continue;
                }

                bool isFlush = groupMessage.Any(r => r is CommandBase);
                var unitOfWork = groupMessage.First().Setting.UnitOfWork;

                if (!unitOfWork.With(r => r.IsOpen()))
                {
                    var unitOfWorkFactory = string.IsNullOrWhiteSpace(groupMessage.Key.DataBaseInstance)
                                                    ? IoCFactory.Instance.TryResolve<IUnitOfWorkFactory>()
                                                    : IoCFactory.Instance.TryResolveByNamed<IUnitOfWorkFactory>(groupMessage.Key.DataBaseInstance);
                    unitOfWork = unitOfWorkFactory.Create(isFlush ? IsolationLevel.ReadCommitted : IsolationLevel.ReadUncommitted, groupMessage.Key.Connection);
                    units.Add(groupMessage.Key.GetHashCode(), new Tuple<IUnitOfWork, bool>(unitOfWork, isFlush));
                }

                foreach (var part in groupMessage)
                {
                    bool isThrow = false;
                    try
                    {
                        part.Setting.UnitOfWork = unitOfWork;
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
                            units.Select(r => r.Value)
                                 .DoEach(tuple => tuple.Item1.Dispose());
                        }
                    }
                }
            }

            units.Select(r => r.Value)
                 .DoEach(tuple =>
                             {
                                 if (tuple.Item2)
                                     tuple.Item1.Commit();

                                 tuple.Item1.Dispose();
                             });

            foreach (var messages in delays.GroupBy(r => r.Setting.Delay, r => r))
            {
                IoCFactory.Instance.TryResolve<IDispatcher>()
                          .Push(new AddDelayToSchedulerCommand
                                    {
                                            Commands = messages
                                                    .ToList()
                                    }, new MessageExecuteSetting
                                           {
                                                   Connection = messages.Key.Connection,
                                                   DataBaseInstance = messages.Key.DataBaseInstance
                                           });
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