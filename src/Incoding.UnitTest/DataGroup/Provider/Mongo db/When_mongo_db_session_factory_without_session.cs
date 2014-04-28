namespace Incoding.UnitTest
{
    #region << Using >>

    using System;
    using System.Configuration;
    using Incoding.Data;
    using Machine.Specifications;
    using NCrunch.Framework;

    #endregion

    [Subject(typeof(MongoDbSessionFactory)), Isolated]
    public class When_mongo_db_session_factory_without_session
    {
        #region Establish value

        static MongoDbSessionFactory mongo;

        static Exception exception;

        #endregion

        Establish establish = () => { mongo = new MongoDbSessionFactory(ConfigurationManager.ConnectionStrings["IncRealMongoDb"].ConnectionString); };

        Because of = () => { exception = Catch.Exception(() => mongo.GetCurrent()); };

        It should_be_ex = () => exception.ShouldBeOfType<InvalidOperationException>();
    }
}