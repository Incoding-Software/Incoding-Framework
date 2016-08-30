namespace Incoding.Data
{
    #region << Using >>

    using System;
    using Raven.Client;

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

        public IDocumentSession Open(string connection)
        {
            currentSession = !string.IsNullOrWhiteSpace(connection)
                                     ? this.documentStore.Value.OpenSession(connection)
                                     : this.documentStore.Value.OpenSession();
            return currentSession;
        }

        #endregion
    }
}