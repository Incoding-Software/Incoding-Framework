namespace Incoding.Data
{
    #region << Using >>

    using System;
    using Raven.Client;
    using Raven.Client.Document;

    #endregion

    public class RavenDbSessionFactory : IRavenDbSessionFactory
    {
        #region Static Fields

        [ThreadStatic]
        static IDocumentSession currentSession;

        #endregion

        #region Fields

        readonly Lazy<IDocumentStore> documentStore;

        #endregion

        #region Constructors

        public RavenDbSessionFactory(IDocumentStore documentStore)
        {
            this.documentStore = new Lazy<IDocumentStore>(documentStore.Initialize);            
        }

        #endregion

        #region IRavenDbSessionFactory Members

        public IDocumentSession GetCurrent()
        {
            if (currentSession == null)
                throw new InvalidOperationException("Database access logic cannot be used, if session not opened. Implicitly session usage not allowed now. Please open session explicitly through UnitOfWorkFactory.Create method");

            return currentSession;
        }

        public IDocumentSession Open(string connection)
        {
            currentSession = !string.IsNullOrWhiteSpace(connection)
                                     ? this.documentStore.Value.OpenSession(connection)
                                     : this.documentStore.Value.OpenSession();
            return currentSession;
        }

        public void Dispose()
        {
            currentSession = null;
        }

        #endregion
    }
}