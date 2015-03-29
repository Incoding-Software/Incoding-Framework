namespace Incoding.Data
{
    using System;

    public interface ISessionFactory<out TSession>
    {
        TSession Open(string connectionString);        
    }
}