namespace Incoding.UnitTest
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics.CodeAnalysis;
    using Incoding.Data;
    using Incoding.Quality;
    using Machine.Specifications.Annotations;

    #endregion

    public class RealDbEntity : IncEntityBase
    {
        #region Fields

        readonly IList<RealDbItemEntity> items = new List<RealDbItemEntity>();

        #endregion

        #region Properties

        public virtual int Value { get; set; }

        public virtual int Value2 { get; set; }

        public virtual string ValueStr { get; set; }

        public virtual int? ValueNullable { get; set; }

        public virtual ReadOnlyCollection<RealDbItemEntity> Items { get { return new ReadOnlyCollection<RealDbItemEntity>(this.items); } }

        public virtual RealDbItemEntity Reference { get; set; }

        #endregion

        #region Api Methods

        public virtual void AddItem(RealDbItemEntity item)
        {
            item.Parent = this;
            this.items.Add(item);
        }

        #endregion

        #region Nested classes

        [UsedImplicitly, Obsolete(ObsoleteMessage.ClassNotForDirectUsage, true), ExcludeFromCodeCoverage]
        public class Map : NHibernateEntityMap<RealDbEntity>
        {
            #region Constructors

            public Map()
            {
                IdGenerateByGuid(r => r.Id);
                Map(r => r.Value);
                Map(r => r.Value2);
                Map(r => r.ValueStr);
                Map(r => r.ValueNullable);
                DefaultHasMany(r => r.Items);
                DefaultReference(r => r.Reference);
            }

            #endregion
        }

        #endregion
    }
}