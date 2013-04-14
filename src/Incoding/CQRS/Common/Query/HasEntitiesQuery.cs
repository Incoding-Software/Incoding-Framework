namespace Incoding.CQRS
{
    #region << Using >>

    using System.Linq;
    using Incoding.Data;

    #endregion

    public class HasEntitiesQuery<TEntity> : QueryBase<IncStructureResponse<bool>> where TEntity : class, IEntity
    {
        protected override IncStructureResponse<bool> ExecuteResult()
        {
            return new IncStructureResponse<bool>(Repository.Query<TEntity>().Any());
        }
    }
}