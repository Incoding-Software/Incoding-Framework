using System.Web.UI.WebControls;

namespace Incoding.Block
{
    #region << Using >>

    using System;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity;
    using System.Data.Entity.ModelConfiguration;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Linq.Expressions;
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

        public class OptionOfDelay
        {
            public OptionOfDelay()
            {
                Async = false;
                TimeOut = 5.Seconds().Milliseconds;
            }

            public bool Async { get; set; }

            public int TimeOut { get; set; }
        }

        public abstract class Sort
        {
            public class Default : OrderSpecification<DelayToScheduler>
            {
                public override Action<AdHocOrderSpecification<DelayToScheduler>> SortedBy()
                {
                    return specification => specification.OrderBy(r => r.Status)
                                                         .OrderBy(r => r.Priority)
                                                         .OrderByDescending(r => r.StartsOn);
                }
            }
        }

        public abstract class Where
        {
            public class ByUID : Specification<DelayToScheduler>
            {
                #region Fields

                readonly string uid;

                #endregion

                #region Constructors

                public ByUID(string uid)
                {
                    this.uid = uid;
                }

                #endregion

                public override Expression<Func<DelayToScheduler, bool>> IsSatisfiedBy()
                {
                    return scheduler => scheduler.UID == this.uid;
                }
            }

            public class AvailableStartsOn : Specification<DelayToScheduler>
            {
                readonly DateTime date;

                public AvailableStartsOn(DateTime date)
                {
                    this.date = date;
                }

                public override Expression<Func<DelayToScheduler, bool>> IsSatisfiedBy()
                {
                    var faultAbove = this.date.AddMinutes(2);
                    return scheduler => scheduler.StartsOn <= faultAbove;
                }
            }

            public class ByAsync : Specification<DelayToScheduler>
            {
                private readonly bool @async;

                public ByAsync(bool @async)
                {
                    this.async = async;
                }

                public override Expression<Func<DelayToScheduler, bool>> IsSatisfiedBy()
                {
                    return scheduler => scheduler.Option.Async == this.async;
                }
            }

            public class ByStatus : Specification<DelayToScheduler>
            {
                #region Fields

                readonly DelayOfStatus[] status;

                #endregion

                public override Expression<Func<DelayToScheduler, bool>> IsSatisfiedBy()
                {
                    return scheduler => status.Contains(scheduler.Status);
                }

                #region Constructors

                public ByStatus(DelayOfStatus status)
                        : this(new[] { status }) { }

                public ByStatus(DelayOfStatus[] status)
                {
                    this.status = status;
                }

                #endregion
            }
        }

        #region Properties

        public new virtual string Id { get; protected set; }

        public virtual string Command { get; set; }

        
        public virtual string Type { get; set; }

        public virtual int Priority { get; set; }

        public virtual DelayOfStatus Status { get; set; }

        public virtual string Description { get; set; }

        public virtual string UID { get; set; }

        public virtual DateTime StartsOn { get; set; }

        public virtual GetRecurrencyDateQuery Recurrence { get; set; }

        public virtual OptionOfDelay Option { get; set; }

        public virtual DateTime? CreateDt { get; set; }

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
                MapEscaping(r => r.CreateDt);  
                Component(r => r.Option, part =>
                                         {
                                             part.Map(r => r.Async, "Option_Async");
                                             part.Map(r => r.TimeOut, "Option_TimeOut");
                                         });
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