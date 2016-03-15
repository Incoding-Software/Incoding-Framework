using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using Incoding.Data;

namespace Incoding.Data
{
    #region << Using >>

    using System.Data.Entity;
    using System.Data.Entity.ModelConfiguration;

    #endregion

    public abstract class EFClassMap<TEntity> : IEFClassMap where TEntity : class
    {
        #region Api Methods

        public virtual void OnModelCreating(DbModelBuilder modelBuilder)
        {
            OnModel(modelBuilder.Entity<TEntity>());
        }

        public abstract void OnModel(EntityTypeConfiguration<TEntity> entity);

        #endregion
    }
}