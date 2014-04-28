namespace Incoding.MvcContrib
{
    #region << Using >>

    using System;
    using System.Linq.Expressions;

    #endregion

    public interface IExecutableSetting
    {
        IExecutableSetting If(Action<IConditionalBuilder> configuration);

        IExecutableSetting If(Expression<Func<bool>> expression);
        
        void TimeOut(double millisecond);

        void TimeOut(TimeSpan time);

        void Interval(double millisecond, out string intervalId);

        void Interval(TimeSpan time, out string intervalId);
    }
}