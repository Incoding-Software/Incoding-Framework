namespace Incoding.Data
{
    #region << Using >>

    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using Incoding.Extensions;
    using JetBrains.Annotations;

    #endregion

    [UsedImplicitly, ExcludeFromCodeCoverage]
    public class MongoDbUnitOfWork : UnitOfWorkBase<MongoDatabaseDisposable, IMongoDbSessionFactory>
    {
        #region Constructors

        public MongoDbUnitOfWork(IMongoDbSessionFactory sessionFactory, IsolationLevel level, string connection)
                : base(sessionFactory, level, connection) { }

        #endregion

        protected override void InternalSubmit() { }

        protected override void InternalFlush() { }

        protected override void InternalCommit() { }

        protected override void InternalOpen()
        {
            this.session.Initialize();
        }
    }
}