namespace Incoding.UnitTest
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;
    using System.Diagnostics.CodeAnalysis;
    using Incoding.Data;
    using Incoding.Quality;
    using Machine.Specifications.Annotations;
    using MongoDB.Bson.Serialization.Attributes;
    using Raven.Imports.Newtonsoft.Json;

    #endregion

    [JsonObject(IsReference = true, ItemReferenceLoopHandling = ReferenceLoopHandling.Ignore)]
    public class DbEntity : IncEntityBase
    {
        #region Constructors

        public DbEntity()
        {
            Id = Guid.NewGuid();
            Items = new List<DbEntityItem>();
        }

        #endregion

        #region Properties

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public new virtual Guid Id { get; set; }

        public virtual string Value { get; set; }

        public virtual int? ValueNullable { get; set; }

        [JsonIgnore]
        public virtual IList<DbEntityItem> Items { get; set; }

        [JsonIgnore]
        public virtual DbEntityReference Reference { get; set; }

        #endregion

        #region Api Methods

        public virtual void AddItem(DbEntityItem entityItem)
        {
            entityItem.Parent = this;
            Items.Add(entityItem);
        }

        #endregion

        #region Nested classes

        [UsedImplicitly, Obsolete(ObsoleteMessage.ClassNotForDirectUsage, true), ExcludeFromCodeCoverage]
        public class Map : NHibernateEntityMap<DbEntity>
        {
            #region Constructors

            public Map()
            {
                Id(r => r.Id).GeneratedBy.Assigned();
                Map(r => r.Value);
                Map(r => r.ValueNullable);
                HasMany(r => r.Items).Cascade.SaveUpdate();
                DefaultReference(r => r.Reference).Nullable();
            }

            #endregion
        }

        [UsedImplicitly, Obsolete(ObsoleteMessage.ClassNotForDirectUsage, true), ExcludeFromCodeCoverage]
        public class EfMap : EFClassMap<DbEntity>
        {
            public override void OnModel(EntityTypeConfiguration<DbEntity> entity)
            {
                entity.HasKey(r => r.Id);
            }
        }

        #endregion
    }
}