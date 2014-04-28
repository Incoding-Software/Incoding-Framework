namespace Incoding.CQRS
{
    #region << Using >>

    using Newtonsoft.Json;

    #endregion

    public interface IMessage<out TResult>
    {        
        TResult Result { get; }
        
        MessageExecuteSetting Setting { get; set; }

        void Execute();
    }
}