namespace Incoding.UnitTest
{
    using Incoding.Block.IoC;
    using Incoding.CQRS;
    using Incoding.Data;
    using Incoding.MSpecContrib;
    using Moq;

    public class Context_message_base_repository
    {
        #region Static Fields

        protected static FakeMessage message;

        protected static Mock<IIoCProvider> provider;

        #endregion

        #region Fake classes

        protected class FakeMessage : MessageBase<object>
        {
            public override void Execute()
            {
                Pleasure.Do10(i => this.Repository.Delete<FakeEntityForNew>(i));
            }
        }

        #endregion

        protected static void Establish()
        {
            provider = Pleasure.MockStrict<IIoCProvider>(mock => mock.Setup(r => r.TryGet<IRepository>()).Returns(Pleasure.MockAsObject<IRepository>()));
            IoCFactory.Instance.Initialize(init => init.WithProvider(provider.Object));
            message = new FakeMessage();
        }
    }
}