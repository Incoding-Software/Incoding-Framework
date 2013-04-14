namespace Incoding.MSpecContrib
{
    using Incoding.CQRS;

    #region << Using >>

    

    #endregion

    public class MockQuery<TMessage, TResult> : MockMessage<TMessage, TResult> where TMessage : MessageBase<TResult> where TResult : class
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