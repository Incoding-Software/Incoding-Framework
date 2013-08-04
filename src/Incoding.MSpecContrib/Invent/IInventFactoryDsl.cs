namespace Incoding.MSpecContrib
{
    #region << Using >>

    using System;
    using System.Linq.Expressions;

    #endregion

    public interface IInventFactoryDsl<T> where T : new()
    {
        IInventFactoryDsl<T> GenerateTo<TGenerate>(Expression<Func<T, TGenerate>> property) where TGenerate : new();

        IInventFactoryDsl<T> GenerateTo<TGenerate>(string property) where TGenerate : new();

        IInventFactoryDsl<T> Empty<TGenerate>(Expression<Func<T, TGenerate>> property);

        IInventFactoryDsl<T> Empty<TGenerate>(string property);

        IInventFactoryDsl<T> Tuning<TValue>(Expression<Func<T, TValue>> property, TValue value);

        IInventFactoryDsl<T> Tuning(string property, object value);

        IInventFactoryDsl<T> Tunings(object propertiesMap);

        IInventFactoryDsl<T> Ignore(Expression<Func<T, object>> property, string reason);

        IInventFactoryDsl<T> IgnoreBecauseAuto(Expression<Func<T, object>> property);

        IInventFactoryDsl<T> Ignore(string property, string reason);

        IInventFactoryDsl<T> Callback(Action<T> callback);
    }
}