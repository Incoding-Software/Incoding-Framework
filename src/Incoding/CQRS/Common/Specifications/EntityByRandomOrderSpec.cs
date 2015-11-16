namespace Incoding.CQRS
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Incoding.Data;
    using JetBrains.Annotations;

    [UsedImplicitly, ExcludeFromCodeCoverage]
    public class EntityByRandomOrderSpec<TEntity> : OrderSpecification<TEntity> where TEntity: IEntity
    {
        public override Action<AdHocOrderSpecification<TEntity>> SortedBy()
        {
            return specification => specification.OrderBy(r => SqlFunctions.NewID());
        }
    }
}