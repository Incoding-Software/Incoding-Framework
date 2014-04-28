namespace Incoding.Data
{
    #region << Using >>

    using Raven.Client;

    #endregion

    public interface IRavenDbSessionFactory : ISessionFactory<IDocumentSession> { }
}