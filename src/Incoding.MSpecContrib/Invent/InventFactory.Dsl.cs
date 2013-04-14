namespace Incoding.MSpecContrib
{
    #region << Using >>

    using System;
    using System.Diagnostics;
    using System.Linq.Expressions;
    using Incoding.Extensions;

    #endregion

    public partial class InventFactory<T> : IInventFactoryDsl<T> where T : new()
    {
        #region IInventFactoryDsl<T> Members

        [DebuggerStepThrough]
        public IInventFactoryDsl<T> GenerateTo<TGenerate>(Expression<Func<T, object>> property) where TGenerate : new()
        {
            return GenerateTo<TGenerate>(property.GetMemberName());
        }

        [DebuggerStepThrough]
        public IInventFactoryDsl<T> GenerateTo<TGenerate>(string property) where TGenerate : new()
        {
            return Tuning(property, Pleasure.Generator.Invent<TGenerate>());
        }

        [DebuggerStepThrough]
        public IInventFactoryDsl<T> Empty<TGenerate>(Expression<Func<T, object>> property)
        {
            return Empty<TGenerate>(property.GetMemberName());
        }

        [DebuggerStepThrough]
        public IInventFactoryDsl<T> Empty<TGenerate>(string property)
        {
            Guard.NotNull("property", property);

            VerifyUniqueProperty(property);
            this.empties.Add(property);

            return this;
        }

        [DebuggerStepThrough]
        public IInventFactoryDsl<T> Tuning(Expression<Func<T, object>> property, object value)
        {
            Guard.NotNull("property", property);
            return Tuning((string)property.GetMemberName(), value);
        }

        [DebuggerStepThrough]
        public IInventFactoryDsl<T> Tuning(string property, object value)
        {
            Guard.NotNull("property", property);

            VerifyUniqueProperty(property);
            this.tuning.Set(property, () => value);
            return this;
        }

        [DebuggerStepThrough]
        public IInventFactoryDsl<T> Ignore(Expression<Func<T, object>> property, string reason)
        {
            Guard.NotNull("property", property);
            return Ignore((string)property.GetMemberName(), reason);
        }

        public IInventFactoryDsl<T> IgnoreBecauseAuto(Expression<Func<T, object>> property)
        {
            return Ignore(property, "Auto");
        }

        [DebuggerStepThrough]
        public IInventFactoryDsl<T> Ignore(string property, string reason)
        {
            Guard.NotNull("property", property);

            VerifyUniqueProperty(property);
            this.ignoreProperties.Add(property);
            return this;
        }

        [DebuggerStepThrough]
        public IInventFactoryDsl<T> Callback(Action<T> callback)
        {
            Guard.NotNull("callback", callback);

            this.callbacks.Add(callback);
            return this;
        }

        public IInventFactoryDsl<T> Tunings(object propertiesMap)
        {
            var dictionary = AnonymousHelper.ToDictionary(propertiesMap);
            foreach (var valuePair in dictionary)
                Tuning((string)valuePair.Key, valuePair.Value);
            return this;
        }

        #endregion
    }
}