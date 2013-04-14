namespace Incoding.MSpecContrib
{
    #region << Using >>

    using System;
    using System.Linq.Expressions;

    #endregion

    // ReSharper disable UnusedMemberInSuper.Global
    public interface ICompareFactoryDsl<TActual, TExpected>
    {
        ICompareFactoryDsl<TActual, TExpected> Forward(Expression<Func<TActual, object>> actualProp, Expression<Func<TExpected, object>> expectedProp);

        ICompareFactoryDsl<TActual, TExpected> Forward(string actualProp, Expression<Func<TExpected, object>> expectedProp);

        ICompareFactoryDsl<TActual, TExpected> ForwardToValue(Expression<Func<TActual, object>> actualProp, object value);

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