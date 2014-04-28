namespace Incoding.UnitTest
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Data.Entity.ModelConfiguration;
    using System.Diagnostics.CodeAnalysis;
    using Incoding.Data;
    using Incoding.Quality;
    using Machine.Specifications.Annotations;
    using Raven.Imports.Newtonsoft.Json;

    #endregion

    public class DbEntityQuery : IncEntityBase
    {
        #region Constructors

        public DbEntityQuery()
        {
            Id = Guid.NewGuid();
            Items = new List<DbEntityQueryAsItem>();
        }

        #endregion

        #region Properties

        public new virtual Guid Id { get; set; }

        public virtual int Value { get; set; }

        public virtual int Value2 { get; set; }

        [JsonIgnore]
        public virtual DbEntityQueryAsReference Reference { get; set; }
        
        [JsonIgnore]
        public virtual IList<DbEntityQueryAsItem> Items { get; set; }

        #endregion

        #region Nested classes

        [UsedImplicitly, Obsolete(ObsoleteMessage.ClassNotForDirectUsage, true), ExcludeFromCodeCoverage]
        public class Map : NHibernateEntityMap<DbEntityQuery>
        {
            #region Constructors

            public Map()
            {
                Id(r => r.Id).GeneratedBy.Assigned();
                Map(r => r.Value);
                Map(r => r.Value2);
                DefaultReference(r => r.Reference);
                HasMany(r => r.Items).Cascade.SaveUpdate();
            }

            #endregion
        }

        [UsedImplicitly, Obsolete(ObsoleteMessage.ClassNotForDirectUsage, true), ExcludeFromCodeCoverage]
        public class EfMap : EFClassMap<DbEntityQuery>
        {
            public override void OnModel(EntityTypeConfiguration<DbEntityQuery> entity)
            {
                entity.HasKey(r => r.Id);
            }
        }

        #endregion
    }
}