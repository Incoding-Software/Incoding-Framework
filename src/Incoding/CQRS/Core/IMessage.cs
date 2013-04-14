namespace Incoding.CQRS
{
    public interface IMessage<out TResult> where TResult : class
    {
        TResult Result { get; }

        void Execute();
    }
}