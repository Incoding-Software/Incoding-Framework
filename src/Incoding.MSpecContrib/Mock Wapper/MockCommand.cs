namespace Incoding.MSpecContrib
{
    #region << Using >>

    using Incoding.CQRS;

    #endregion

    public class MockCommand<TMessage> : MockMessage<TMessage, object> where TMessage : MessageBase<object>
    {
        #region Constructors

        protected MockCommand(TMessage instanceMessage)
                : base(instanceMessage) { }

        #endregion

        #region Factory method

        public static MockMessage<TMessage, object> When(TMessage instanceMessage)
        {
            return new MockCommand<TMessage>(instanceMessage);
        }

        #endregion
    }
}