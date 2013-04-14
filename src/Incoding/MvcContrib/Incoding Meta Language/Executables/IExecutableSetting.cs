namespace Incoding.MvcContrib
{
    #region << Using >>

    using System;

    #endregion

    public interface IExecutableSetting
    {
        IExecutableSetting If(Action<IConditionalBuilder> configuration);

        void TimeOut(double millisecond);

        void Interval(double millisecond, out string intervalId);
    }
}