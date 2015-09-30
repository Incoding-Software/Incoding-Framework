namespace Incoding.Data
{
    #region << Using >>

    using System;

    #endregion

    public interface IUnitOfWork : IDisposable            
    {
        void Flush();

        void Commit();

        IRepository GetRepository();
    }
}