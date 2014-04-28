namespace Incoding.UnitTest
{
    #region << Using >>

    using System;
    using Incoding.Data;
    using Incoding.Extensions;
    using Incoding.MSpecContrib;
    using NHibernate;

    #endregion

    public static class RepositoryTestExtensions
    {
        #region Factory constructors

        public static void Clear(this IRepository repository)
        {
            repository.Flush();
            if (repository is NhibernateRepository)
            {
                var session = repository.TryGetValue("session") as ISession;
                session.Clear();
            }
        }

        public static void Close(this IRepository repository, Action doWithoutSession)
        {
            if (repository is NhibernateRepository)
            {
                var session = repository.TryGetValue("session") as ISession;
                session.Close();
                doWithoutSession();
                repository.SetValue("session", session.SessionFactory.OpenSession());
            }
        }

        public static bool IsNH(this IRepository repository)
        {
            return repository is NhibernateRepository;
        }

        public static bool IsMongo(this IRepository repository)
        {
            return repository is MongoDbRepository;
        }

        public static void Init(this IRepository repository)
        {
            foreach (var entity in repository.Query<DbEntity>())
                repository.Delete(entity);
            
            foreach (var entity in repository.Query<DbEntityByInt>())
                repository.Delete(entity);
            
            foreach (var entity in repository.Query<DbEntityAsGuid>())
                repository.Delete(entity);

            foreach (var entity in repository.Query<DbEntityReference>())
                repository.Delete(entity);

            foreach (var entity in repository.Query<DbEntityItem>())
                repository.Delete(entity);

            foreach (var entity in repository.Query<DbEntityQuery>())
                repository.Delete(entity);

            foreach (var entity in repository.Query<DbEntityReference>())
                repository.Delete(entity);

            foreach (var entity in repository.Query<DbEntityQueryAsItem>())
                repository.Delete(entity);

            repository.Flush();

            Pleasure.Do10((i) => repository.Save(Pleasure.Generator.Invent<DbEntityQuery>(dsl => dsl.GenerateTo(r => r.Reference)
                                                                                                    .Callback(query =>
                                                                                                                  {
                                                                                                                      var item = Pleasure.Generator.Invent<DbEntityQueryAsItem>(factoryDsl => factoryDsl.Tuning(r => r.Parent, query));
                                                                                                                      query.Items.Add(item);
                                                                                                                  }))));
            repository.Flush();
            Pleasure.Sleep100Milliseconds(); // wait for apply data base.
        }

        #endregion
    }
}