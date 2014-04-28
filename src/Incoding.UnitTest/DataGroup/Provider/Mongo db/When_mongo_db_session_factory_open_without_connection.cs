namespace Incoding.UnitTest
{
    #region << Using >>

    using System.Configuration;
    using Incoding.Data;
    using Incoding.Extensions;
    using Machine.Specifications;
    using NCrunch.Framework;

    #endregion

    [Subject(typeof(MongoDbSessionFactory)), Isolated]
    public class When_mongo_db_session_factory_open_without_connection
    {
        #region Establish value

        static MongoDbSessionFactory mongo;

        static MongoDatabaseDisposable session;

        #endregion

        Establish establish = () => { mongo = new MongoDbSessionFactory(ConfigurationManager.ConnectionStrings["IncRealMongoDb"].ConnectionString); };

        Because of = () => { session = mongo.Open(null); };

        It should_be_open = () => session.ShouldNotBeNull();

        It should_be_set_current = () => mongo.TryGetValue("currentSession").ShouldBeTheSameAs(session);

        It should_be_get_current = () => mongo.GetCurrent().ShouldBeTheSameAs(session);
    }
}