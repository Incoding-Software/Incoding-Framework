namespace Incoding.UnitTest
{
    #region << Using >>

    using System.Configuration;
    using System.Linq;
    using FluentNHibernate;
    using FluentNHibernate.Cfg;
    using FluentNHibernate.Cfg.Db;
    using Incoding.Data;
    using Incoding.MSpecContrib;
    using Machine.Specifications;
    using NHibernate;
    using NHibernate.Context;
    using NHibernate.Linq;

    #endregion

    public class Context_nhibernate_repository_query 
    {
        #region Estabilish value

        protected static NhibernateRepository repository;

        protected static ISession Session;

        #endregion

        #region Fields

        Establish establish = () =>
                                  {
                                      var msSql = Fluently
                                              .Configure()
                                              .Database(MsSqlConfiguration.MsSql2008
                                                                          .ConnectionString(ConfigurationManager.ConnectionStrings["IncRealDb"].ConnectionString)
                                                                          .ShowSql())
                                              .Mappings(configuration => configuration.FluentMappings.AddFromAssembly(typeof(RealDbEntity).Assembly))
                                              .CurrentSessionContext<ThreadStaticSessionContext>();

                                      var sessionSource = new SessionSource(msSql);
                                      Session = sessionSource.CreateSession();

                                      foreach (var del in Session.Query<RealDbEntity>())
                                          Session.Delete(del);
                                      Pleasure.Do10((i) => Session.Save(Pleasure.Generator.InventEntity<RealDbEntity>(dsl => dsl
                                                                                                                                     .Tuning(r => r.Reference, Pleasure.Generator.InventEntity<RealDbItemEntity>())
                                                                                                                                     .Callback(entity =>
                                                                                                                                                   {
                                                                                                                                                       for (int j = 0; j < i + 1; j++)
                                                                                                                                                           entity.AddItem(Pleasure.Generator.InventEntity<RealDbItemEntity>());
                                                                                                                                                   }))));
                                      Session.Flush();

                                      var nhibernateSessionFactory = Pleasure.MockAsObject<INhibernateSessionFactory>(mock => mock.Setup(r => r.GetCurrentSession()).Returns(Session));
                                      repository = new NhibernateRepository(nhibernateSessionFactory);
                                  };

        #endregion
    }
}