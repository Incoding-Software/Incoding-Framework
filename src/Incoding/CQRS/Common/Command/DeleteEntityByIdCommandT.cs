namespace Incoding.CQRS
{
    #region << Using >>

    using System;
    using System.Diagnostics.CodeAnalysis;
    using Incoding.Data;
    using Incoding.Quality;
    using JetBrains.Annotations;

    #endregion

    public class DeleteEntityByIdCommand<TEntity> : DeleteEntityByIdCommand where TEntity : IEntity
    {
        #region Constructors

        [UsedImplicitly, Obsolete(ObsoleteMessage.SerializeConstructor), ExcludeFromCodeCoverage]
        public DeleteEntityByIdCommand() { }

        public DeleteEntityByIdCommand(string id)
                : base(id, typeof(TEntity)) { }

        #endregion
    }
}