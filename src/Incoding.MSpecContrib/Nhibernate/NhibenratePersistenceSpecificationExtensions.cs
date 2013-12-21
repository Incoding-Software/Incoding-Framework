namespace Incoding.MSpecContrib
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using FluentNHibernate.Testing;
    using FluentNHibernate.Testing.Values;
    using FluentNHibernate.Utils;
    using Incoding.Extensions;
    using Incoding.Maybe;
    using Incoding.Quality;
    using Machine.Specifications;

    #endregion

    public static class NhibernatePersistenceSpecificationExtensions
    {
        #region Factory constructors

        public static TEntity VerifyMappingAndSchema<TEntity>(this PersistenceSpecification<TEntity> persistenceSpecification, Action<SpecWithPersistenceSpecification<TEntity>.Setting> configuration = null) where TEntity : class, new()
        {
            var setting = new SpecWithPersistenceSpecification<TEntity>.Setting();
            configuration.Do(action => action(setting));

            var allAssignField = ((List<Property<TEntity>>)persistenceSpecification.TryGetValue("allProperties"))
                    .Select(r => (Accessor)r.TryGetValue("PropertyAccessor"))
                    .Select(r => r.FieldName);

            var duplicateProperties = allAssignField.Where(r => allAssignField.Count(s => s == r) > 1)
                                                    .Distinct()
                                                    .ToList();

            if (duplicateProperties.Any())
                throw new SpecificationException("Duplicate fields: {0}".F(string.Join(",", duplicateProperties)));

            var forgetProperties = (from propertyInfo in typeof(TEntity).GetProperties(setting.BindingFlags)
                                    where !propertyInfo.HasAttribute<IgnoreCompareAttribute>()
                                    where !setting.IgnoreProperties.Select(r => r.GetMemberName()).Any(r => r.Equals(propertyInfo.Name, StringComparison.InvariantCultureIgnoreCase))
                                    where !allAssignField.Any(r => r.Equals(propertyInfo.Name))
                                    select propertyInfo.Name).ToList();

            if (forgetProperties.Any())
                throw new SpecificationException("Not found fields: {0}".F(string.Join(",", forgetProperties)));

            return persistenceSpecification.VerifyTheMappings(setting.PreEntity ?? new TEntity());
        }

        #endregion
    }
}