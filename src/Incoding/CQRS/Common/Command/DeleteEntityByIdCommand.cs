namespace Incoding.CQRS
{
    #region << Using >>

    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Incoding.Data;
    using Incoding.Extensions;
    using Incoding.Quality;
    using JetBrains.Annotations;

    #endregion

    public class DeleteEntityByIdCommand : CommandBase
    {
        #region Constructors

        [UsedImplicitly, Obsolete(ObsoleteMessage.SerializeConstructor), ExcludeFromCodeCoverage]
        public DeleteEntityByIdCommand() { }

        public DeleteEntityByIdCommand(string id, Type type)
        {
            Id = id;
            AssemblyQualifiedName = type.AssemblyQualifiedName;
        }

        #endregion

        #region Properties

        public string Id { get; set; }

        public string AssemblyQualifiedName { get; set; }

        #endregion

        #region Override

        protected override void Execute()
        {
            var entityType = Type.GetType(AssemblyQualifiedName);

            if (entityType == null)
                throw new IncFrameworkException("Type {0} not found".F(AssemblyQualifiedName));

            var allRepositoryMethod = Repository.GetType().GetMethods();

            var entity = allRepositoryMethod
                    .First(r => r.Name.EqualsWithInvariant("GetById"))
                    .MakeGenericMethod(entityType)
                    .Invoke(Repository, new object[] { Id }) as IEntity;

            allRepositoryMethod
                    .First(r => r.Name.EqualsWithInvariant("Delete") && r.GetParameters()[0].Name.EqualsWithInvariant("entity")).MakeGenericMethod(entityType)
                    .Invoke(Repository, new object[] { entity });
        }

        #endregion
    }
}