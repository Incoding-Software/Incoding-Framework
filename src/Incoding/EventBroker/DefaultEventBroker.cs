namespace Incoding.EventBroker
{
    #region << Using >>

    using System;
    using System.Linq;
    using System.Reflection;
    using Incoding.Block.ExceptionHandling;
    using Incoding.Block.IoC;
    using Incoding.Extensions;
    using Incoding.Maybe;

    #endregion

    public class DefaultEventBroker : IEventBroker
    {
        #region Fields

        ActionPolicy defaultActionPolicy;

        Func<IEventSubscriber<IEvent>> sharedSubscriber;

        #endregion

        #region Constructors

        public DefaultEventBroker()
        {
            this.defaultActionPolicy = ActionPolicy.Direct();
        }

        #endregion

        #region IEventBroker Members

        public void Publish<TEvent>(TEvent @event) where TEvent : class, IEvent
        {
            this.sharedSubscriber.Do(subscriber => subscriber().Subscribe(@event));

            var eventType = @event.GetType();

            var allSubscriberTypes = IoCFactory.Instance.ResolveAll<object>(typeof(IEventSubscriber<>).MakeGenericType(new[] { eventType }));

            foreach (var currentHandler in allSubscriberTypes)
            {
                var handleMethod = currentHandler
                        .GetType()
                        .GetMethods()
                        .First(r => r.Name.Equals("Subscribe") && r.GetParameters().Any(parameterInfo => parameterInfo.ParameterType == eventType));

                var eventHandler = currentHandler;

                Action handleEvent = () =>
                                         {
                                             try
                                             {
                                                 handleMethod.Invoke(eventHandler, new object[] { @event });
                                             }
                                             catch (TargetInvocationException invocationException)
                                             {
                                                 var actualException = invocationException.InnerException;
                                                 throw actualException;
                                             }
                                             finally
                                             {
                                                 ((IDisposable)eventHandler).Dispose();
                                             }
                                         };

                var descriptionAttribute = handleMethod.FirstOrDefaultAttribute<HandlerDescriptionAttribute>();
                bool isSync = descriptionAttribute == null || !descriptionAttribute.IsAsync;
                if (isSync)
                    this.defaultActionPolicy.Do(handleEvent.Invoke);
                else
                {
                    this.defaultActionPolicy.Do(() =>
                                                    {
                                                        var asyncResult = handleEvent.BeginInvoke(null, handleEvent);
                                                        if (descriptionAttribute.IsWait)
                                                        {
                                                            asyncResult.AsyncWaitHandle.WaitOne();
                                                            asyncResult.AsyncWaitHandle.Close();
                                                        }
                                                    });
                }
            }
        }

        public bool HasSubscriber<TEvent>(TEvent onAfterErrorExecuteEvent) where TEvent : IEvent
        {
            var subscriberType = typeof(IEventSubscriber<>).MakeGenericType(new[] { typeof(TEvent) });
            return IoCFactory.Instance
                             .ResolveAll<object>(subscriberType)
                             .Any();
        }

        #endregion

        #region Api Methods

        public DefaultEventBroker WithSharedSubscriber(Func<IEventSubscriber<IEvent>> evaluated)
        {
            this.sharedSubscriber = evaluated;
            return this;
        }

        public DefaultEventBroker WithActionPolicy(ActionPolicy actionPolicy)
        {
            this.defaultActionPolicy = actionPolicy;
            return this;
        }

        #endregion
    }
}