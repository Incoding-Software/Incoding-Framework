namespace Incoding.MSpecContrib
{
    using Incoding.CQRS;

    #region << Using >>

    

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