namespace Incoding.CQRS
{
    #region << Using >>

    using System;
    using JetBrains.Annotations;

    #endregion

    [UsedImplicitly]
    public interface ISetUp : IDisposable
    {
        int GetOrder();

        void Execute();
    }
}