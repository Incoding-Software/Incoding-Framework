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

    public class DbEntityWithSpecificIdName : IncEntityBase
    {
        #region Constructors

        public DbEntityWithSpecificIdName()
        {
            Id = Guid.NewGuid().ToString();
        }

        #endregion

        #region Properties

        public new virtual string Id { get; set; }

        #endregion

        #region Nested classes

        [UsedImplicitly, Obsolete(ObsoleteMessage.ClassNotForDirectUsage, true), ExcludeFromCodeCoverage]
        public class Map : NHibernateEntityMap<DbEntityWithSpecificIdName>
        {
            #region Constructors

            public Map()
            {
                Id(r => r.Id)
                        .GeneratedBy
                        .Assigned()
                        .Column("NotId");
            }

            #endregion
        }

        [UsedImplicitly, Obsolete(ObsoleteMessage.ClassNotForDirectUsage, true), ExcludeFromCodeCoverage]
        public class EfMap : EFClassMap<DbEntityWithSpecificIdName>
        {
            public override void OnModel(EntityTypeConfiguration<DbEntityWithSpecificIdName> entity)
            {
                entity.HasKey(r => r.Id);
            }
        }

        #endregion
    }
}