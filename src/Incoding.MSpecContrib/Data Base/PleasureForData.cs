namespace Incoding.MSpecContrib
{
    #region << Using >>

    using System;
    using System.Data.Entity;
    using System.Reflection;
    using System.Windows.Forms;
    using FluentNHibernate.Cfg;
    using Incoding.Data;
    using Incoding.Extensions;
    using MongoDB.Bson.Serialization;
    using MongoDB.Driver;
    using NHibernate;
    using Raven.Client.Document;
    using Raven.Imports.Newtonsoft.Json.Serialization;

    #endregion

    public static class PleasureForData
    {
        #region Factory constructors

        public static IRepository BuildEFRepository(IncDbContext dbContext, bool reloadDb = true)
        {
            try
            {
                if (reloadDb)
                {
                    Database.SetInitializer(new CreateDatabaseIfNotExists<IncDbContext>());
                    dbContext.Database.CreateIfNotExists();
                }
                else
                    Database.SetInitializer(new NullDatabaseInitializer<IncDbContext>());

                return new EntityFrameworkRepository(dbContext);
            }
                    
                    ////ncrunch: no coverage start
            catch (Exception e)
            {
                Clipboard.SetText("Exception in  build configuration {0}".F(e));
                return null;
            }

            ////ncrunch: no coverage end      
        }

        public static IRepository BuildMongoDbRepository(string url, bool reload = true)
        {
            var mongoUrl = new MongoUrl(url);
            var db = new MongoClient(mongoUrl).GetServer();
            if (reload)
            {
                if (db.DatabaseExists(mongoUrl.DatabaseName))
                    db.DropDatabase(mongoUrl.DatabaseName);
            }

            if (!BsonClassMap.IsClassMapRegistered(typeof(IncEntityBase)))
                BsonClassMap.RegisterClassMap<IncEntityBase>(map => map.UnmapProperty(r => r.Id));
            var session = new MongoDatabaseDisposable(db.GetDatabase(mongoUrl.DatabaseName));

            return new MongoDbRepository(session);
        }

        public static IRepository BuildNhibernateRepository()
        {
            try
            {
                var session = SessionFactory.Open(null);
                session.CacheMode = CacheMode.Ignore;
                return new NhibernateRepository(session);
            }
                    
                    ////ncrunch: no coverage start
            catch (Exception e)
            {
                Clipboard.SetText("Exception in  build configuration {0}".F(e));
                return null;
            }

            ////ncrunch: no coverage end  
        }

        public static IRepository BuildRavenDbRepository(DocumentStore document)
        {
            var documentSession = document.Initialize().OpenSession();
            document.Conventions.JsonContractResolver = new DefaultContractResolver(shareCache: true)
                                                        {
                                                                DefaultMembersSearchFlags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.SetField | BindingFlags.SetProperty, 
                                                        };
            return new RavenDbRepository(documentSession);
        }

        public static void StartEF(IncDbContext dbContext, bool reloadDb = true)
        {
            SpecWithRepository.Repository = BuildEFRepository(dbContext, reloadDb);
        }

        public static void StartNhibernate(FluentConfiguration instanceBuilderConfiguration, bool reloadDb = true)
        {
            if (reloadDb)
            {
                IManagerDataBase managerDataBase = new NhibernateManagerDataBase(instanceBuilderConfiguration);
                if (!managerDataBase.IsExist())
                    managerDataBase.Create();

                managerDataBase.Update();
            }

            SessionFactory = new NhibernateSessionFactory(instanceBuilderConfiguration);

            SpecWithRepository.Repository = BuildNhibernateRepository();
        }

        #endregion

        #region Properties

        public static NhibernateSessionFactory SessionFactory { get; set; }

        #endregion
    }
}