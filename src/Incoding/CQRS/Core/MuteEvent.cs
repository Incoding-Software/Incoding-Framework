namespace Incoding.CQRS
{
    using System;

    [Flags]
    public enum MuteEvent
    {
        OnError = 1, 

        OnBefore = 2, 

        OnAfter = 4, 

        OnComplete = 8
    }
}