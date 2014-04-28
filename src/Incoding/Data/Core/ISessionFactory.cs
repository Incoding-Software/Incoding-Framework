namespace Incoding.Data
{
    using System;

    public interface ISessionFactory<TSession>:IDisposable
    {
        TSession GetCurrent();

        TSession Open(string connectionString);        
    }
}