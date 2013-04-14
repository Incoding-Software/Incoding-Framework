namespace Incoding.EventBroker
{
    #region << Using >>

    using System;

    #endregion

    [AttributeUsage(AttributeTargets.Method)]
    public sealed class HandlerAsyncWaitAttribute : HandlerDescriptionAttribute
    {
        #region Constructors

        public HandlerAsyncWaitAttribute()
                : base(true, true) { }

        #endregion
    }
}