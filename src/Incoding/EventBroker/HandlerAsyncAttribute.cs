namespace Incoding.EventBroker
{
    #region << Using >>

    using System;

    #endregion

    [AttributeUsage(AttributeTargets.Method)]
    public sealed class HandlerAsyncAttribute : Attribute { }
}