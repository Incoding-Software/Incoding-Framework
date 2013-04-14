namespace Incoding.MSpecContrib
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using FluentNHibernate.Testing;
    using FluentNHibernate.Testing.Values;
    using FluentNHibernate.Utils;
    using Incoding.Data;
    using Incoding.Extensions;
    using Incoding.Maybe;
    using Machine.Specifications;
    using NHibernate;

    #endregion

    public static class NHibernateMSpecExtensions
    {
        #region Factory constructors

        public static void DeleteForever<TEntity>(this ISession session, string id) where TEntity : IEntity
        {
            var entityFromDb = session.Get<TEntity>(id);
            session.DeleteForever(entityFromDb);
        }

        public static void DeleteForever<TEntity>(this ISession session, TEntity entity) where TEntity : IEntity
        {
            session.Delete(entity);
            Submit(session);
        }

        public static TEntity ReSave<TEntity>(this ISession session, IEntity entity) where TEntity : IEntity
        {
            session.SubmitChanges(entity);
            var reload = session.Get<TEntity>(entity.Id);
            session.Close();
            return reload;
        }

        public static void ShouldBeDelete<TEntity>(this ISession session, TEntity entity) where TEntity : IEntity
        {
            session.Get<TEntity>(entity.Id).ShouldBeNull();
        }

        public static void ShouldContains<TEntity>(this ISession session, TEntity entity) where TEntity : IEntity
        {
            session.Get<TEntity>(entity.Id).ShouldNotBeNull();
        }

        public static void ShouldNotBeDelete<TEntity>(this ISession session, TEntity entity) where TEntity : IEntity
        {
            session.ShouldContains(entity);
        }

        public static void Submit(this ISession session)
        {
            session.Flush();
            session.Clear();
        }

        public static void SubmitChanges<TEntity>(this ISession session, TEntity entity)
        {
            session.Save(entity);
            Submit(session);
        }

        public static TEntity VerifyMappingAndSchema<TEntity>(this PersistenceSpecification<TEntity> persistenceSpecification, Action<SpecWithPersistenceSpecification<TEntity>.Setting> configuration = null) where TEntity : class
        {
            var setting = new SpecWithPersistenceSpecification<TEntity>.Setting();
            configuration.Do(action => action(setting));

            var allAssignField = ((List<Property<TEntity>>)persistenceSpecification.TryGetValue("allProperties"))
                    .Select(r => (Accessor)r.TryGetValue("PropertyAccessor"))
                    .Select(r => r.FieldName);

            var allForgetProperties = (from propertyInfo in typeof(TEntity).GetProperties(setting.BindingFlags)
                                       where !propertyInfo.Name.Equals("Id")
                                       where !setting.IgnoreProperties.Select(r => r.GetMemberName()).Any(r => r.Equals(propertyInfo.Name, StringComparison.InvariantCultureIgnoreCase))
                                       where !allAssignField.Any(r => r.Equals(propertyInfo.Name))
                                       select propertyInfo.Name).ToList();

            if (allForgetProperties.Count > 0)
                throw new SpecificationException("Not found fields: {0}".F(allForgetProperties.Aggregate(string.Empty, (s, r) => s += ";" + r)));

            return setting.PreEntity
                          .ReturnOrDefault(persistenceSpecification.VerifyTheMappings, persistenceSpecification.VerifyTheMappings());
        }

        #endregion
    }
}