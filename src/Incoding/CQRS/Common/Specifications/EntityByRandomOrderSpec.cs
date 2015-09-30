namespace Incoding.CQRS
{
    using System;
    using Incoding.Data;

    public class EntityByRandomOrderSpec<TEntity> : OrderSpecification<TEntity> where TEntity: IEntity
    {
        public override Action<AdHocOrderSpecification<TEntity>> SortedBy()
        {
            return specification => specification.OrderBy(r => SqlFunctions.NewID());
        }
    }
}