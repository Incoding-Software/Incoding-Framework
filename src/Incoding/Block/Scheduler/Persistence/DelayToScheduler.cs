namespace Incoding.Block
{
    #region << Using >>

    using System;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;
    using System.Diagnostics.CodeAnalysis;
    using Incoding.CQRS;
    using Incoding.Data;
    using Incoding.Extensions;
    using Incoding.Quality;
    using JetBrains.Annotations;
    using Raven.Imports.Newtonsoft.Json;

    #endregion

    public class DelayToScheduler : IncEntityBase
    {
        #region Constructors

        public DelayToScheduler()
        {
            Id = Guid.NewGuid().ToString();
        }

        #endregion

        #region Properties

        public new virtual string Id { get; protected set; }

        public virtual string Command { get; set; }

        [IgnoreCompare("Auto"), JsonIgnore]
        public virtual CommandBase Instance { get { return Command.DeserializeFromJson(System.Type.GetType(Type)) as CommandBase; } }

        public virtual string Type { get; set; }

        public virtual string GroupKey { get; set; }

        public virtual int Priority { get; set; }

        public virtual DelayOfStatus Status { get; set; }

        public virtual string Description { get; set; }

        public virtual string UID { get; set; }

        #endregion

        #region Nested classes

        [UsedImplicitly, ExcludeFromCodeCoverage]
        public class Map : NHibernateEntityMap<DelayToScheduler>
        {
            #region Constructors

            protected Map()
            {
                Id(r => r.Id).GeneratedBy.Assigned();
                MapEscaping(r => r.Command);
                MapEscaping(r => r.Type);
                MapEscaping(r => r.GroupKey);
                MapEscaping(r => r.Priority);
                MapEscaping(r => r.UID);
                MapEscaping(r => r.Description);
                MapEscaping(r => r.Status).CustomType<DelayOfStatus>();
            }

            #endregion
        }

        [UsedImplicitly, Obsolete(ObsoleteMessage.ClassNotForDirectUsage, true), ExcludeFromCodeCoverage]
        public class EfMap : EFClassMap<DelayToScheduler>
        {
            public override void OnModel(EntityTypeConfiguration<DelayToScheduler> entity)
            {
                entity.HasKey(r => r.Id)
                      .Property(r => r.Id)
                      .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            }
        }

        #endregion
    }
}