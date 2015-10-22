namespace Incoding.MSpecContrib
{
    #region << Using >>

    using Incoding.CQRS;

    #endregion

    public class MockQuery<TMessage, TResult> : MockMessage<TMessage, TResult> where TMessage : MessageBase
    {
        #region Constructors

        protected MockQuery(TMessage instanceMessage)
                : base(instanceMessage) { }

        #endregion

        #region Factory constructors

        public static MockMessage<TMessage, TResult> When(TMessage instanceMessage)
        {
            return new MockQuery<TMessage, TResult>(instanceMessage);
        }

        #endregion
    }
}