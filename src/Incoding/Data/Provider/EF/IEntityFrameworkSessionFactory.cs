namespace Incoding.Data
{
    #region << Using >>

    using System.Data.Entity;

    #endregion

    public interface IEntityFrameworkSessionFactory : ISessionFactory<DbContext>
    {        
    }
}