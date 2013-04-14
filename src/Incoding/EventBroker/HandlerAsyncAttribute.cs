namespace Incoding.EventBroker
{
    #region << Using >>

    using System;

    #endregion

    [AttributeUsage(AttributeTargets.Method)]
    public sealed class HandlerAsyncAttribute : HandlerDescriptionAttribute
    {
        #region Constructors

        public HandlerAsyncAttribute()
                : base(true, false) { }

        #endregion
    }
}