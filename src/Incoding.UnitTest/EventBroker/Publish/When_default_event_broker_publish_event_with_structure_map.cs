namespace Incoding.UnitTest
{
    #region << Using >>

    using Incoding.Block.IoC;
    using Incoding.EventBroker;
    using Machine.Specifications;
    using StructureMap.Configuration.DSL;
    using StructureMap.Graph;

    #endregion

    [Subject(typeof(DefaultEventBroker))]
    public class When_default_event_broker_publish_event_with_structure_map : Behaviors_default_event_broker_publish_event
    {
        #region Fake classes

        protected class StructureMapSubscriber : Registry
        {
            #region Constructors

            public StructureMapSubscriber()
            {
                Scan(r =>
                         {
                             r.TheCallingAssembly();
                             r.WithDefaultConventions();
                             r.ConnectImplementationsToTypesClosing(typeof(IEventSubscriber<>));
                         });
            }

            #endregion
        }

        #endregion

        Establish establish = () => Establish(new StructureMapIoCProvider(new StructureMapSubscriber()));

        Because of = () =>
                         {
                             eventBroker.Publish(new FakeEvent());
                             eventBroker.Publish(new FakeEvent2());
                         };

        Behaves_like<Behaviors_default_event_broker_publish_event> verify;
    }
}