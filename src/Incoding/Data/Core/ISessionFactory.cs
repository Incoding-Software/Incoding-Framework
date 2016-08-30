namespace Incoding.Data
{
    #region << Using >>

    using System;

    #endregion

    public interface ISessionFactory<out TSession> 
    {
        TSession Open(string connectionString);
    }
}