namespace Incoding.UnitTest
{
    #region << Using >>

    using System.Data;
    using System.Data.SqlClient;
    using Incoding.Data;
    using Incoding.MSpecContrib;
    using Machine.Specifications;
    using Moq;
    using NCrunch.Framework;
    using NHibernate;
    using It = Machine.Specifications.It;

    #endregion

    [Subject(typeof(NhibernateSessionFactory)), Isolated]
    public class When_nhibernate_session_factory_open_session_with_connection_string
    {
        #region Establish value

        static NhibernateSessionFactory nhSessionFactory;

        static string connectionString;

        static Mock<IDbConnection> connection;

        #endregion

        Establish establish = () =>
                                  {
                                      connectionString = Pleasure.Generator.Invent<SqlConnection>().ConnectionString;
                                      connection = Pleasure.Mock<IDbConnection>();
                                      var session = Pleasure.MockStrict<ISession>(mock => mock.SetupGet(r => r.Connection).Returns(connection.Object));
                                      nhSessionFactory = new NhibernateSessionFactory(Pleasure.MockStrictAsObject<ISessionFactory>(mock => mock.Setup(r => r.OpenSession()).Returns(session.Object)));
                                  };

        Because of = () => nhSessionFactory.Open(connectionString);

        It should_be_set_connection_string = () => connection.VerifySet(r => r.ConnectionString = connectionString);
    }
}