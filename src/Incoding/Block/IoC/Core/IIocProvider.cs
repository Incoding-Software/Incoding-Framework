namespace Incoding.Block.IoC
{
    #region << Using >>

    using System;
    using System.Collections.Generic;

    #endregion

    public interface IIoCProvider : IDisposable
    {
        #region Methods

        void Eject<TInstance>();

        void Forward<TNew>(TNew newInstance) where TNew : class;

        TInstance Get<TInstance>(Type type) where TInstance : class;

        IEnumerable<TInstance> GetAll<TInstance>(Type typeInstance);

        TInstance TryGet<TInstance>() where TInstance : class;

        TInstance TryGet<TInstance>(Type type) where TInstance : class;

        TInstance TryGetByNamed<TInstance>(string named) where TInstance : class;

        #endregion
    }
}