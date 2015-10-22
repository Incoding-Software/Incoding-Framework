using NHibernate.Util;

namespace Incoding.MSpecContrib
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using Incoding.Data;
    using Incoding.Extensions;
    using Incoding.Maybe;
    using Machine.Specifications;

    #endregion

    public class PersistenceSpecification<TEntity> where TEntity : class, IEntity, new()
    {
        #region Fields

        readonly List<string> ignoreProperties = new List<string>
                                                 {
                                                         "Id"
                                                 };

        readonly IRepository repository;

        readonly List<string> properties;

        BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.SetProperty;

        TEntity preEntity;

        TEntity original;

        IUnitOfWork unitOfWork;

        #endregion

        #region Constructors

        public PersistenceSpecification(IRepository repository = null)
        {
            if (repository != null)
                this.repository = repository;
            else
            {
                this.unitOfWork = PleasureForData.Factory.Value.Create(IsolationLevel.ReadUncommitted, true);
                this.repository = this.unitOfWork.GetRepository();
            }
            this.original = new TEntity();
            this.properties = new List<string>();
        }

        public IRepository Repository { get { return this.repository; } }

        #endregion

        #region Api Methods

        public PersistenceSpecification<TEntity> CheckProperty<TValue>(Expression<Func<TEntity, TValue>> prop, TValue value, Action<TEntity, TValue> configure)
        {
            this.properties.Add(prop.GetMemberName());
            configure(this.original, value);
            return this;
        }

        public PersistenceSpecification<TEntity> CheckProperty<TValue>(Expression<Func<TEntity, IEnumerable<TValue>>> prop, IEnumerable<TValue> values, Action<TEntity, TValue> configure)
        {
            this.properties.Add(prop.GetMemberName());
            foreach (var item in values)
                configure(this.original, item);
            return this;
        }

        public PersistenceSpecification<TEntity> CheckProperty<TValue>(Expression<Func<TEntity, TValue>> prop, TValue value)
        {
            return CheckProperty(prop, value, (entity, value2) => entity.SetValue(prop.GetMemberName(), value2));
        }

        public PersistenceSpecification<TEntity> CheckProperty<TValue>(Expression<Func<TEntity, TValue>> prop) where TValue : new()
        {
            return CheckProperty(prop, Pleasure.Generator.Invent<TValue>(dsl =>
                                                                         {
                                                                             if (typeof(TValue).IsImplement<IEntity>())
                                                                                 dsl.Ignore("Id", "auto");
                                                                         }));
        }

        public void VerifyMappingAndSchema(Action<PersistenceSpecification<TEntity>> configure = null)
        {
            configure.Do(action => action(this));

            if (this.preEntity == null)
            {
                string allDuplicate = this.properties.Where(r => this.properties.Count(s => s == r) > 1)
                        .Distinct()
                        .AsString(",");
                if (!string.IsNullOrWhiteSpace(allDuplicate))
                    throw new SpecificationException("Duplicate fields:{0}".F(allDuplicate));

                string allCheckInIgnore = this.ignoreProperties.Where(r => this.properties.Contains(r))
                        .Distinct()
                        .AsString(",");
                if (!string.IsNullOrWhiteSpace(allCheckInIgnore))
                    throw new SpecificationException("Fields:{0} was ignore and can't check".F(allCheckInIgnore));

                var allMissing = this.original.GetType()
                        .GetProperties(this.bindingFlags)
                        .Where(r => r.CanWrite)
                        .Where(s => !this.properties.Contains(s.Name) && !this.ignoreProperties.Contains(s.Name))
                        .Where(r => !r.Name.EqualsWithInvariant("Id"));

                foreach (var missing in allMissing)
                {
                    object invent;
                    if (missing.PropertyType.IsImplement<IEntity>())
                    {
                        invent = ((IQueryable)Repository.GetType().GetMethod("Query").MakeGenericMethod(missing.PropertyType).Invoke(Repository, new object[] { null, null, null, null })).FirstOrNull();
                        if (invent == null)
                            throw new SpecificationException("No elements at database for type '{0}'".F(missing.PropertyType.Name));
                    }
                    else
                        invent = Pleasure.Generator.Invent(missing.PropertyType);

                    missing.SetValue(this.original, invent, null);
                }
            }
            else
                this.original = this.preEntity;

            this.Repository.Save(this.original);
            this.Repository.Flush();

            object id = this.original.TryGetValue("Id");
            var entityFromDb = this.Repository.GetById<TEntity>(id);
            if (entityFromDb == null)
                throw new SpecificationException("Can't found entity {0} by id {1}".F(typeof(TEntity).Name, id));

            entityFromDb.ShouldEqualWeak(this.original, dsl =>
                                                        {
                                                            foreach (var ignoreProperty in this.ignoreProperties)
                                                                dsl.Ignore(ignoreProperty, "Fixed");
                                                            dsl.IgnoreRecursionError();
                                                            dsl.SetMaxRecursionDeep(0); // set to 0 for a while to simplify checking properties
                                                        });

            this.unitOfWork.Dispose();
        }

        public PersistenceSpecification<TEntity> IgnoreBaseClass()
        {
            this.bindingFlags = this.bindingFlags | BindingFlags.DeclaredOnly;
            return this;
        }

        public PersistenceSpecification<TEntity> WithEntity(TEntity entity)
        {
            this.preEntity = entity;
            return this;
        }

        public PersistenceSpecification<TEntity> IgnoreProperty(Expression<Func<TEntity, object>> ignoreProperty, string reason)
        {
            string memberName = ignoreProperty.GetMemberName();
            if (!this.ignoreProperties.Contains(memberName))
                this.ignoreProperties.Add(memberName);
            return this;
        }

        public PersistenceSpecification<TEntity> IgnoreBecauseCalculate(Expression<Func<TEntity, object>> ignoreProperty)
        {
            return IgnoreProperty(ignoreProperty, "Calculate");
        }

        #endregion
    }
}