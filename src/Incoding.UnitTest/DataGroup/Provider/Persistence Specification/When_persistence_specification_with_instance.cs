namespace Incoding.UnitTest.MSpecGroup
{
    #region << Using >>

    using System.Configuration;
    using System.Data;
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

                                            new PersistenceSpecification<DbEntity>(PleasureForData.BuildEFSessionFactory(dbContext).Create(IsolationLevel.ReadUncommitted, true))
                                                    .CheckProperty(r => r.Value, Pleasure.Generator.String())
                                                    .CheckProperty(r => r.ValueNullable, Pleasure.Generator.PositiveNumber())
                                                    .CheckProperty(r => r.Reference)
                                                    .CheckProperty(r => r.Items, Pleasure.ToList(Pleasure.Generator.Invent<DbEntityItem>()), (entity, itemEntity) => entity.AddItem(itemEntity))
                                                    .VerifyMappingAndSchema();
                                        };

        It should_be_mongo_db = () => new PersistenceSpecification<DbEntity>(PleasureForData.BuildMongoDb(ConfigurationManager.ConnectionStrings["IncRealMongoDb"].ConnectionString).Create(IsolationLevel.ReadCommitted, true))
                                              .CheckProperty(r => r.Value, Pleasure.Generator.String())
                                              .CheckProperty(r => r.ValueNullable, Pleasure.Generator.PositiveNumber())
                                              .CheckProperty(r => r.Reference)
                                              .CheckProperty(r => r.Items, Pleasure.ToList(Pleasure.Generator.Invent<DbEntityItem>()), (entity, itemEntity) => entity.AddItem(itemEntity))
                                              .VerifyMappingAndSchema();

        It should_be_nhibernate = () => new PersistenceSpecification<DbEntity>()
                                                .CheckProperty(r => r.Value, Pleasure.Generator.String())
                                                .CheckProperty(r => r.ValueNullable, Pleasure.Generator.PositiveNumber())
                                                .CheckProperty(r => r.Reference)
                                                .CheckProperty(r => r.Items, Pleasure.ToList(Pleasure.Generator.Invent<DbEntityItem>()), (entity, itemEntity) => entity.AddItem(itemEntity))
                                                .VerifyMappingAndSchema();

        It should_be_nhibernate_without_mapping = () => Catch.Exception(() => new PersistenceSpecification<DbEntityWithoutMapping>()
                                                                                      .VerifyMappingAndSchema())
                                                             .ShouldBeAssignableTo<InternalSpecificationException>();

        It should_be_raven_db = () => new PersistenceSpecification<DbEntity>(PleasureForData.BuildRavenDb(new DocumentStore
                                                                                                          {
                                                                                                                  Url = ConfigurationManager.ConnectionStrings["IncRealRavenDb"].ConnectionString,
                                                                                                                  DefaultDatabase = "IncTest",
                                                                                                          }).Create(IsolationLevel.ReadCommitted))
                                              .CheckProperty(r => r.Value, Pleasure.Generator.String())
                                              .CheckProperty(r => r.ValueNullable, Pleasure.Generator.PositiveNumber())
                                              .CheckProperty(r => r.Reference)
                                              .CheckProperty(r => r.Items, Pleasure.ToList(Pleasure.Generator.Invent<DbEntityItem>()), (entity, itemEntity) => entity.AddItem(itemEntity))
                                              .VerifyMappingAndSchema();
    }
}