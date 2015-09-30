namespace Incoding.CQRS
{
    #region << Using >>

    using Incoding.Data;
    using Newtonsoft.Json;

    #endregion

    public interface IMessage<out TResult>
    {        
        TResult Result { get; }
        
        MessageExecuteSetting Setting { get; set; }

        void OnExecute(IDispatcher current, IUnitOfWork unitOfWork);
    }
}