namespace Incoding.Data
{
    #region << Using >>

    using System;

    #endregion

    public interface IUnitOfWork : IDisposable            
    {
        void Flush();

        IRepository GetRepository();

        void Commit();
    }
}