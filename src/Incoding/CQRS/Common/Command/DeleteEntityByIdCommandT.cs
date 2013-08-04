namespace Incoding.CQRS
{
    using Incoding.Data;

    public class DeleteEntityByIdCommand<TEntity> : DeleteEntityByIdCommand where TEntity : IEntity
    {
        #region Constructors

        public DeleteEntityByIdCommand(string id)
                : base(id, typeof(TEntity)) { }

        #endregion
    }
}