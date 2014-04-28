namespace Incoding.CQRS
{
    #region << Using >>

    using System.Linq;
    using Incoding.Data;

    #endregion

    public class HasEntitiesQuery<TEntity> : QueryBase<IncBoolResponse> where TEntity : class, IEntity, new()
    {
        protected override IncBoolResponse ExecuteResult()
        {
            return Repository.Query<TEntity>().Any();
        }
    }
}