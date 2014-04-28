namespace Incoding.UnitTest
{
    #region << Using >>

    using System;
    using System.Data.Entity.ModelConfiguration;
    using System.Diagnostics.CodeAnalysis;
    using Incoding.Data;
    using Incoding.Quality;
    using Machine.Specifications.Annotations;

    #endregion

    public class DbEntityAsGuid : IncEntityBase
    {
        #region Constructors

        public DbEntityAsGuid()
        {
            Id = Guid.NewGuid();
        }

        #endregion

        #region Properties

        public new virtual Guid Id { get; set; }

        #endregion

        #region Nested classes

        [UsedImplicitly, Obsolete(ObsoleteMessage.ClassNotForDirectUsage, true), ExcludeFromCodeCoverage]
        public class Map : NHibernateEntityMap<DbEntityAsGuid>
        {
            #region Constructors

            public Map()
            {
                Id(r => r.Id).GeneratedBy.Assigned();
            }

            #endregion
        }

        [UsedImplicitly, Obsolete(ObsoleteMessage.ClassNotForDirectUsage, true), ExcludeFromCodeCoverage]
        public class EfMap : EFClassMap<DbEntityAsGuid>
        {
            public override void OnModel(EntityTypeConfiguration<DbEntityAsGuid> entity)
            {
                entity.ToTable("DbEntityAsGuid_Tbl");
            }
        }

        #endregion
    }
}