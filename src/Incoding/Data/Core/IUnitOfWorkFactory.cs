namespace Incoding.Data
{
    #region << Using >>

    using System.Data;

    #endregion

    public interface IUnitOfWorkFactory
    {
        IUnitOfWork Create(IsolationLevel level, string connection = null);
    }
}