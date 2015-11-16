namespace Incoding.CQRS
{
    #region << Using >>

    using System;
    using Incoding.Data;
    using Newtonsoft.Json;

    #endregion

    public interface IMessage 
    {
        object Result { get; }
        
        MessageExecuteSetting Setting { get; set; }

        void OnExecute(IDispatcher current, Lazy<IUnitOfWork> unitOfWork);
    }
}