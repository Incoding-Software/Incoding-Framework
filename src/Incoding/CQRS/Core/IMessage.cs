namespace Incoding.CQRS
{
    public interface IMessage<out TResult>
    {
        TResult Result { get; }

        void Execute();
    }
}