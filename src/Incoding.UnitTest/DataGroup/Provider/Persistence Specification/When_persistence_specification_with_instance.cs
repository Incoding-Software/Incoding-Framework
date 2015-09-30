namespace Incoding.UnitTest.MSpecGroup
{
    #region << Using >>

    using System.Configuration;
    using Incoding.Data;
    using Incoding.MSpecContrib;
    using Machine.Specifications;
    using Raven.Client.Document;

    #endregion

    [Subject(typeof(PersistenceSpecification<>))]
    public class When_persistence_specification_with_instance
    {
        It should_be_by_start_assembly = () => new PersistenceSpecification<DbEntity>()
                                                       .CheckProperty(r => r.Value, Pleasure.Generator.String())
                                                       .CheckProperty(r => r.ValueNullable, Pleasure.Generator.PositiveNumber())
                                                       .CheckProperty(r => r.Reference)
                                                       .CheckProperty(r => r.Items, Pleasure.ToList(Pleasure.Generator.Invent<DbEntityItem>()), (entity, itemEntity) => entity.AddItem(itemEntity))
                                                       .VerifyMappingAndSchema();

        It should_be_entity_framework = () =>
                                            {
                                                var dbContext = new IncDbContext("IncRealEFDb", typeof(DbEntity).Assembly);
                                                dbContext.Configuration.ValidateOnSaveEnabled = false;
                                                dbContext.Configuration.LazyLoadingEnabled = true;

                                                new PersistenceSpecification<DbEntity>(PleasureForData.BuildEFRepository(dbContext))
                                                        .CheckProperty(r => r.Value, Pleasure.Generator.String())
                                                        .CheckProperty(r => r.ValueNullable, Pleasure.Generator.PositiveNumber())
                                                        .CheckProperty(r => r.Reference)
                                                        .CheckProperty(r => r.Items, Pleasure.ToList(Pleasure.Generator.Invent<DbEntityItem>()), (entity, itemEntity) => entity.AddItem(itemEntity))
                                                        .VerifyMappingAndSchema();
                                            };

        It should_be_nhibernate = () => new PersistenceSpecification<DbEntity>(PleasureForData.BuildNhibernateRepository())
                                                .CheckProperty(r => r.Value, Pleasure.Generator.String())
                                                .CheckProperty(r => r.ValueNullable, Pleasure.Generator.PositiveNumber())
                                                .CheckProperty(r => r.Reference)
                                                .CheckProperty(r => r.Items, Pleasure.ToList(Pleasure.Generator.Invent<DbEntityItem>()), (entity, itemEntity) => entity.AddItem(itemEntity))
                                                .VerifyMappingAndSchema();

        It should_be_nhibernate_without_mapping = () => new PersistenceSpecification<DbEntityWithoutMapping>(PleasureForData.BuildNhibernateRepository())
                                                                .VerifyMappingAndSchema();

        It should_be_mongo_db = () => new PersistenceSpecification<DbEntity>(PleasureForData.BuildMongoDbRepository(ConfigurationManager.ConnectionStrings["IncRealMongoDb"].ConnectionString))
                                              .CheckProperty(r => r.Value, Pleasure.Generator.String())
                                              .CheckProperty(r => r.ValueNullable, Pleasure.Generator.PositiveNumber())
                                              .CheckProperty(r => r.Reference)
                                              .CheckProperty(r => r.Items, Pleasure.ToList(Pleasure.Generator.Invent<DbEntityItem>()), (entity, itemEntity) => entity.AddItem(itemEntity))
                                              .VerifyMappingAndSchema();

        It should_be_raven_db = () => new PersistenceSpecification<DbEntity>(PleasureForData.BuildRavenDbRepository(new DocumentStore
                                                                                                                        {
                                                                                                                            Url = ConfigurationManager.ConnectionStrings["IncRealRavenDb"].ConnectionString,
                                                                                                                                DefaultDatabase = "IncTest",
                                                                                                                        }))
                                              .CheckProperty(r => r.Value, Pleasure.Generator.String())
                                              .CheckProperty(r => r.ValueNullable, Pleasure.Generator.PositiveNumber())
                                              .CheckProperty(r => r.Reference)
                                              .CheckProperty(r => r.Items, Pleasure.ToList(Pleasure.Generator.Invent<DbEntityItem>()), (entity, itemEntity) => entity.AddItem(itemEntity))
                                              .VerifyMappingAndSchema();
    }
}