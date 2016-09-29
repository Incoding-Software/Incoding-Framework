namespace Incoding.MSpecContrib
{
    #region << Using >>

    using System;
    using System.Data;
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
        public static Lazy<IUnitOfWorkFactory> Factory = null;

        #region Factory constructors

        public static IUnitOfWorkFactory BuildEFSessionFactory(IncDbContext dbContext, bool reloadDb = true)
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

                return new EntityFrameworkUnitOfWorkFactory(new EntityFrameworkSessionFactory(() => dbContext));
            }
                    
                    ////ncrunch: no coverage start
            catch (Exception e)
            {
                Clipboard.SetText("Exception in  build configuration {0}".F(e));
                return null;
            }

            ////ncrunch: no coverage end      
        }

        
        public static IUnitOfWorkFactory BuildMongoDb(string url, bool reload = true)
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
            
            return new MongoDbUnitOfWorkFactory(new MongoDbSessionFactory(url));
        }

        public static IUnitOfWorkFactory BuildNhibernateUnitOfWorkFactory(Func<FluentConfiguration> instanceBuilderConfigurationAction, bool reloadDb)
        {
            try
            {
                var builderConfiguration = instanceBuilderConfigurationAction();
                if (reloadDb)
                {
                    IManagerDataBase managerDataBase = new NhibernateManagerDataBase(builderConfiguration);
                    if (!managerDataBase.IsExist())
                        managerDataBase.Create();

                    managerDataBase.Update();
                }


                var unitOfWork = new NhibernateUnitOfWorkFactory(new NhibernateSessionFactory(builderConfiguration));
                return unitOfWork;
            }
                    
            ////ncrunch: no coverage start
            catch (Exception e)
            {
                Clipboard.SetText("Exception in  build configuration {0}".F(e));
                return null;
            }

            ////ncrunch: no coverage end  
        }

        public static RavenDbUnitOfWorkFactory BuildRavenDb(DocumentStore document)
        {
            
            return new RavenDbUnitOfWorkFactory(new RavenDbSessionFactory(document));
        }

        public static void StartEF(IncDbContext dbContext, bool reloadDb = true)
        {
            Factory = new Lazy<IUnitOfWorkFactory>(() => BuildEFSessionFactory(dbContext, reloadDb));
        }

        public static void StartNhibernate(Func<FluentConfiguration> instanceBuilderConfigurationAction, bool reloadDb = true)
        {
            Factory = new Lazy<IUnitOfWorkFactory>(() => BuildNhibernateUnitOfWorkFactory(instanceBuilderConfigurationAction, reloadDb));
        }

        #endregion
    }
}