namespace Incoding.MSpecContrib
{
    #region << Using >>

    using System;
    using System.Linq.Expressions;

    #endregion

    // ReSharper disable UnusedMemberInSuper.Global
    public interface ICompareFactoryDsl<TActual, TExpected>
    {
        ICompareFactoryDsl<TActual, TExpected> Forward<TValue>(Expression<Func<TActual, TValue>> actualProp, Expression<Func<TExpected, TValue>> expectedProp);

        ICompareFactoryDsl<TActual, TExpected> Forward<TValue>(string actualProp, Expression<Func<TExpected, TValue>> expectedProp);

        ICompareFactoryDsl<TActual, TExpected> ForwardToValue<TValue>(Expression<Func<TActual, TValue>> actualProp, TValue value);

        ICompareFactoryDsl<TActual, TExpected> ForwardToValue(string actualProp, object value);

        ICompareFactoryDsl<TActual, TExpected> ForwardToAction(Expression<Func<TActual, object>> actualProp, Action<TActual> predicate);

        ICompareFactoryDsl<TActual, TExpected> ForwardToAction(string actualProp, Action<TActual> predicate);

        ICompareFactoryDsl<TActual, TExpected> Ignore(Expression<Func<TActual, object>> actualIgnore, string reason);

        ICompareFactoryDsl<TActual, TExpected> IgnoreBecauseNotUse(Expression<Func<TActual, object>> actualIgnore);

        ICompareFactoryDsl<TActual, TExpected> IgnoreBecauseRoot(Expression<Func<TActual, object>> actualIgnore);

        ICompareFactoryDsl<TActual, TExpected> IgnoreBecauseCalculate(Expression<Func<TActual, object>> actualIgnore);

        ICompareFactoryDsl<TActual, TExpected> Ignore(string actualIgnore, string reason);

        ICompareFactoryDsl<TActual, TExpected> IncludeAllFields();
    }

    // ReSharper restore UnusedMemberInSuper.Global
}