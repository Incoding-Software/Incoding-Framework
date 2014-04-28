namespace Incoding.MSpecContrib
{
    #region << Using >>

    using System;
    using System.Linq.Expressions;
    using Incoding.Extensions;

    #endregion

    public partial class InventFactory<T> : IInventFactoryDsl<T>
    {
        
        #region IInventFactoryDsl<T> Members

        public IInventFactoryDsl<T> GenerateTo<TGenerate>(Expression<Func<T, TGenerate>> property) where TGenerate : new()
        {
            return GenerateTo(property, null);
        }

        public IInventFactoryDsl<T> GenerateTo<TGenerate>(Expression<Func<T, TGenerate>> property, Action<IInventFactoryDsl<TGenerate>> innerDsl) where TGenerate : new()
        {
            return Tuning(property, Pleasure.Generator.Invent(innerDsl));
        }

        public IInventFactoryDsl<T> Empty<TGenerate>(Expression<Func<T, TGenerate>> property)
        {
            return Empty<TGenerate>(property.GetMemberName());
        }

        public IInventFactoryDsl<T> Empty<TGenerate>(string property)
        {
            Guard.NotNull("property", property);

            VerifyUniqueProperty(property);
            this.empties.Add(property);

            return this;
        }

        public IInventFactoryDsl<T> Tuning<TValue>(Expression<Func<T, TValue>> property, TValue value)
        {
            Guard.NotNull("property", property);
            return Tuning(property.GetMemberName(), value);
        }

        public IInventFactoryDsl<T> Tuning(string property, object value)
        {
            Guard.NotNull("property", property);

            VerifyUniqueProperty(property);
            this.tunings.Set(property, () => value);
            return this;
        }

        public IInventFactoryDsl<T> Ignore(Expression<Func<T, object>> property, string reason)
        {
            Guard.NotNull("property", property);
            return Ignore(property.GetMemberName(), reason);
        }

        public IInventFactoryDsl<T> IgnoreBecauseAuto(Expression<Func<T, object>> property)
        {
            return Ignore(property, "Auto");
        }

        public IInventFactoryDsl<T> Ignore(string property, string reason)
        {
            Guard.NotNull("property", property);

            VerifyUniqueProperty(property);
            this.ignoreProperties.Add(property);
            return this;
        }

        public IInventFactoryDsl<T> Callback(Action<T> callback)
        {
            Guard.NotNull("callback", callback);

            this.callbacks.Add(callback);
            return this;
        }

        public IInventFactoryDsl<T> MuteCtor()
        {
            this.isMuteCtor = true;
            return this;
        }

        public IInventFactoryDsl<T> Tunings(object propertiesMap)
        {
            var dictionary = AnonymousHelper.ToDictionary(propertiesMap);
            foreach (var valuePair in dictionary)
                Tuning(valuePair.Key, valuePair.Value);
            return this;
        }

        #endregion
    }
}