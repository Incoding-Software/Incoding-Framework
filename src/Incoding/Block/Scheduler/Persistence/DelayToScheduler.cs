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
    using NHibernate.Util;
    using System.Linq;

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

        public virtual DateTime StartsOn { get; set; }

        public virtual Reccurence Recurrence { get; set; }

        #endregion

        #region Nested classes

        public class Reccurence
        {

            #region Properties

            public virtual RepeatOfType Repeats { get; set; }

            public virtual int? RepeatEvery { get; set; }

            public virtual DateTime? StartsOn { get; set; }

            public virtual DateTime? EndsOfDt { get; set; }

            public virtual int? EndsOfAfter { get; set; }

            public virtual DayOfWeek? RepeatOn { get; set; }

            #endregion

            #region Api Methods

            
            public virtual DateTime? NextDt()
            {
                DateTime startsOn = StartsOn.GetValueOrDefault(DateTime.UtcNow);
                if (Repeats == RepeatOfType.Daily)
                    StartsOn = startsOn.AddDays(RepeatEvery.GetValueOrDefault());
                else if (Repeats == RepeatOfType.Weekly)
                {
                    var allOurDaysOfWeek = Enum.GetValues(typeof(DayOfWeek)).Cast<DayOfWeek>();
                    
                    if (!RepeatOn.HasValue)
                        this.RepeatOn = allOurDaysOfWeek.FirstOrDefault(r => r.ToString() == startsOn.DayOfWeek.ToString());

                    var allRepeatOn = allOurDaysOfWeek.Where(r => RepeatOn.Value.HasFlag(r));
                    var allDotNetDaysOfWeek = Enum.GetValues(typeof(System.DayOfWeek))
                                                  .Cast<System.DayOfWeek>()
                                                  .Where(r => allRepeatOn.Any(s => s.ToString() == r.ToString()))
                                                  .ToList();




                }



                if (EndsOfDt.HasValue)
                {
                    if (startsOn > EndsOfDt)
                        return null;
                }

                if (EndsOfAfter.HasValue)
                {
                    EndsOfAfter -= 1;
                    if (EndsOfAfter + 1 == 0)
                        return null;
                }

                return startsOn;
            }

            #endregion
        }

        [UsedImplicitly, ExcludeFromCodeCoverage]
        public class Map : NHibernateEntityMap<DelayToScheduler>
        {
            #region Constructors

            protected Map()
            {
                Id(r => r.Id).GeneratedBy.Assigned();
                MapEscaping(r => r.Command).CustomType("StringClob").CustomSqlType("nvarchar(max)");
                MapEscaping(r => r.Type).CustomType("StringClob").CustomSqlType("nvarchar(max)");
                MapEscaping(r => r.GroupKey);
                MapEscaping(r => r.Priority);
                MapEscaping(r => r.UID);
                MapEscaping(r => r.Description).CustomType("StringClob").CustomSqlType("nvarchar(max)");
                MapEscaping(r => r.Status).CustomType<DelayOfStatus>();
                MapEscaping(r => r.StartsOn);
                Component(r => r.Recurrence, part =>
                                             {
                                                 part.Map(r => r.EndsOfAfter, "Recurrence_EndsOfAfter");
                                                 part.Map(r => r.EndsOfDt, "Recurrence_EndsOfDt");
                                                 part.Map(r => r.RepeatEvery, "Recurrence_RepeatEvery");
                                                 part.Map(r => r.RepeatOn, "Recurrence_RepeatOn").CustomType<DayOfWeek?>();
                                                 part.Map(r => r.Repeats, "Recurrence_Repeats").CustomType<RepeatOfType>();
                                                 part.Map(r => r.StartsOn, "Recurrence_StartsOn");
                                             });
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

        #region Enums

        [Flags]
        public enum DayOfWeek
        {
            Sunday = 1,

            Monday = 2,

            Tuesday = 4,

            Wednesday = 6,

            Thursday = 8,

            Friday = 16,

            Saturday = 32,
        }

        public enum RepeatOfType
        {
            None,

            Daily,

            Weekly,

            Monthly,

            Yearly
        }

        #endregion
    }
}