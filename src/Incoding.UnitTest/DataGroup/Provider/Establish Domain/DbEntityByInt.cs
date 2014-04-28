namespace Incoding.UnitTest
{
    #region << Using >>

    using System;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;
    using System.Diagnostics.CodeAnalysis;
    using Incoding.Data;
    using Incoding.Quality;
    using Machine.Specifications.Annotations;

    #endregion

    public class DbEntityByInt : IncEntityBase
    {
        #region Properties

        public new virtual int Id { get; set; }

        #endregion

        #region Nested classes

        [UsedImplicitly, Obsolete(ObsoleteMessage.ClassNotForDirectUsage, true), ExcludeFromCodeCoverage]
        public class Map : NHibernateEntityMap<DbEntityByInt>
        {
            #region Constructors

            public Map()
            {
                Id(r => r.Id).GeneratedBy.Increment();
            }

            #endregion
        }

        [UsedImplicitly, Obsolete(ObsoleteMessage.ClassNotForDirectUsage, true), ExcludeFromCodeCoverage]
        public class EfMap : EFClassMap<DbEntityByInt>
        {
            public override void OnModel(EntityTypeConfiguration<DbEntityByInt> entity)
            {
                entity.HasKey(r => r.Id)
                      .Property(r => r.Id)
                      .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            }
        }

        #endregion
    }
}