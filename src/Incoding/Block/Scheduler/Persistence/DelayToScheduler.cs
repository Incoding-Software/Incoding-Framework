namespace Incoding.Block
{
    #region << Using >>

    using System;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity;
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
        
        public virtual int Priority { get; set; }

        public virtual DelayOfStatus Status { get; set; }

        public virtual string Description { get; set; }

        public virtual string UID { get; set; }

        public virtual DateTime StartsOn { get; set; }

        public virtual GetRecurrencyDateQuery Recurrence { get; set; }

        #endregion

        #region Nested classes

        [UsedImplicitly, ExcludeFromCodeCoverage]
        public class Map : NHibernateEntityMap<DelayToScheduler>
        {
            #region Constructors

            protected Map()
            {
                Id(r => r.Id).GeneratedBy.Assigned();
                MapEscaping(r => r.Command).CustomType("StringClob").CustomSqlType("nvarchar(max)");
                MapEscaping(r => r.Type).CustomType("StringClob").CustomSqlType("nvarchar(max)");                
                MapEscaping(r => r.Priority);
                MapEscaping(r => r.UID);
                MapEscaping(r => r.Description).CustomType("StringClob").CustomSqlType("nvarchar(max)");
                MapEscaping(r => r.Status).CustomType<DelayOfStatus>();
                MapEscaping(r => r.StartsOn);
                Component(r => r.Recurrence, part =>
                                             {
                                                 part.Map(r => r.EndDate, "Recurrence_EndDate");
                                                 part.Map(r => r.RepeatCount, "Recurrence_RepeatCount");
                                                 part.Map(r => r.RepeatDays, "Recurrence_RepeatDays").CustomType<GetRecurrencyDateQuery.DayOfWeek?>();
                                                 part.Map(r => r.RepeatInterval, "Recurrence_RepeatInterval");
                                                 part.Map(r => r.StartDate, "Recurrence_StartDate");
                                                 part.Map(r => r.Type, "Recurrence_Type").CustomType<GetRecurrencyDateQuery.RepeatType?>();
                                             });
            }

            #endregion
        }

        [UsedImplicitly, Obsolete(ObsoleteMessage.ClassNotForDirectUsage, true), ExcludeFromCodeCoverage]
        public class EfMap : EFClassMap<DelayToScheduler>
        {
            public override void OnModelCreating(DbModelBuilder modelBuilder)
            {
                modelBuilder.ComplexType<GetRecurrencyDateQuery>()
                            .Ignore(r => r.Result)
                            .Ignore(r => r.Setting);
                base.OnModelCreating(modelBuilder);
            }

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