namespace Incoding.Data
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using FluentNHibernate.Mapping;
    using Incoding.Extensions;

    #endregion

    ////ncrunch: no coverage start
    public class NHibernateEntityMap<TEntity> : ClassMap<TEntity>
    {
        #region Constructors

        protected NHibernateEntityMap()
        {
            TableEscaping(typeof(TEntity));
        }

        #endregion

        protected void TableEscaping(Type type)
        {
            Table("{0}_Tbl".F(type.Name));
        }

        protected ManyToManyPart<TChild> DefaultHasManyToMany<TChild>(Expression<Func<TEntity, IEnumerable<TChild>>> property)
        {
            return HasManyToMany(property)
                    .Access.CamelCaseField()
                    .Cascade.None()
                    .LazyLoad();
        }

        protected ManyToOnePart<TReference> DefaultReference<TReference>(Expression<Func<TEntity, TReference>> property)
        {
            return References(property)
                    .LazyLoad()
                    .Cascade.SaveUpdate();
        }

        protected OneToManyPart<TChild> DefaultHasMany<TChild>(Expression<Func<TEntity, IEnumerable<TChild>>> property)
        {
            return HasMany(property)
                    .Access.CamelCaseField()
                    .Cascade.AllDeleteOrphan()
                    .LazyLoad();
        }

        protected PropertyPart MapEscaping(Expression<Func<TEntity, object>> property)
        {
            return Map(property, "{0}_Col".F(property.GetMemberName()));
        }

        protected IdentityPart IdGenerateByGuid(Expression<Func<TEntity, object>> property)
        {
            return Id(property)
                    .CustomType<string>()
                    .GeneratedBy
                    .Custom(typeof(GuidStringGenerator))
                    .Length(36);
        }
    }

    ////ncrunch: no coverage end
}