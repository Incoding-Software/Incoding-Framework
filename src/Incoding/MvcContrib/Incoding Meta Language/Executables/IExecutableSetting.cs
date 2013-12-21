namespace Incoding.MvcContrib
{
    #region << Using >>

    using System;

    #endregion

    public interface IExecutableSetting
    {
        IExecutableSetting If(Action<IConditionalBuilder> configuration);
        
        void TimeOut(double millisecond);

        void TimeOut(TimeSpan time);

        void Interval(double millisecond, out string intervalId);

        void Interval(TimeSpan time, out string intervalId);
    }
}