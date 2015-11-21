namespace Incoding.UnitTest
{
    #region << Using >>

    using Incoding.Block.IoC;
    using Incoding.EventBroker;
    using Machine.Specifications;
    using Ninject.Extensions.Conventions;
    using Ninject.Modules;

    #endregion

    [Subject(typeof(DefaultEventBroker))]
    public class When_default_event_broker_publish_event_with_ninject : Behaviors_default_event_broker_publish_event
    {
        #region Fake classes

        protected class NinjectSubscriber : NinjectModule
        {
            public override void Load()
            {
                Kernel.Bind(syntax => syntax
                                              .FromThisAssembly()
                                              .IncludingNonePublicTypes()
                                              .SelectAllClasses()
                                              .InheritedFrom(typeof(IEventSubscriber<>))
                                              .BindAllInterfaces());
            }
        }

        #endregion

        Establish establish = () => Establish(new NinjectIoCProvider(new NinjectSubscriber()));

        Because of = () =>
                         {
                             eventBroker.Publish(new FakeEvent());
                             eventBroker.Publish(new FakeEvent2());
                         };

        Behaves_like<Behaviors_default_event_broker_publish_event> verify;
    }
}