namespace Incoding.Data
{
    #region << Using >>

    using System;

    #endregion

    public interface IUnitOfWork : IDisposable
    {
        bool IsOpen();

        void Flush();

        void Commit();

        void Open();
    }
}