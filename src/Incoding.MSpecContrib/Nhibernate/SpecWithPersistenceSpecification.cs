namespace Incoding.MSpecContrib
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Reflection;
    using FluentNHibernate.Testing;
    using Machine.Specifications;

    #endregion

    public class SpecWithPersistenceSpecification<TEntity> : SpecWithNHibernateSession where TEntity : class
    {
        #region Static Fields

        protected static PersistenceSpecification<TEntity> persistenceSpecification;

        #endregion

        #region Fields

        Establish establish = () =>
                                  {
                                      if (Session != null)
                                          persistenceSpecification = new PersistenceSpecification<TEntity>(Session);
                                  };

        #endregion

        #region Nested classes

        public class Setting
        {
            #region Fields

            internal readonly List<Expression<Func<TEntity, object>>> IgnoreProperties = new List<Expression<Func<TEntity, object>>>();

            internal BindingFlags BindingFlags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.SetProperty;

            internal TEntity PreEntity;

            #endregion

            #region Api Methods

            public Setting IgnoreBaseClass()
            {
                this.BindingFlags = this.BindingFlags | BindingFlags.DeclaredOnly;
                return this;
            }

            public Setting WithEntity(TEntity entity)
            {
                this.PreEntity = entity;
                return this;
            }

            public Setting IgnoreProperty(Expression<Func<TEntity, object>> ignoreProperty, string reason)
            {
                this.IgnoreProperties.Add(ignoreProperty);
                return this;
            }

            public Setting IgnoreBecauseCalculate(Expression<Func<TEntity, object>> ignoreProperty)
            {
                return IgnoreProperty(ignoreProperty, "Calculate");
            }

            #endregion
        }

        #endregion
    }
}