namespace Incoding.Data
{
    #region << Using >>

    using System.Data;

    #endregion

    public interface IUnitOfWorkFactory
    {
        IUnitOfWork Create(IsolationLevel level, bool isFlush, string connection = null);
    }
}