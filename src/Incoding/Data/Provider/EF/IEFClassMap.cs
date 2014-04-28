namespace Incoding.Data
{
    using System.Data.Entity;

    public interface IEFClassMap
    {
        void OnModelCreating(DbModelBuilder modelBuilder);
    }
}