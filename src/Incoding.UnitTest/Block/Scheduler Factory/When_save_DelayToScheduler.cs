namespace Incoding.UnitTest
{
    #region << Using >>

    using Incoding.Block;
    using Incoding.Data;
    using Incoding.MSpecContrib;
    using Machine.Specifications;
    using NCrunch.Framework;
    using Raven.Client.Document;

    #endregion

    [Subject(typeof(DelayToScheduler)),Isolated]
    public class When_save_DelayToScheduler : SpecWithPersistenceSpecification<DelayToScheduler>
    {
        It should_be_nhibernate = () => new PersistenceSpecification<DelayToScheduler>(PleasureForData.BuildNhibernateRepository(MSpecAssemblyContext.NhibernateFluent()))
                                                .VerifyMappingAndSchema(specification => specification.IgnoreBecauseCalculate(r => r.Instance));

        It should_be_ef = () => new PersistenceSpecification<DelayToScheduler>(PleasureForData.BuildEFRepository(new IncDbContext("IncRealEFDb", typeof(DelayToScheduler).Assembly), false))
                                        .VerifyMappingAndSchema(specification => specification.IgnoreBecauseCalculate(r => r.Instance));

        It should_be_raven_db = () => new PersistenceSpecification<DelayToScheduler>(PleasureForData.BuildRavenDbRepository(new DocumentStore
                                                                                                                                {
                                                                                                                                        Url = "http://localhost:8080/", 
                                                                                                                                        DefaultDatabase = "IncTest", 
                                                                                                                                }))
                                              .VerifyMappingAndSchema(specification => specification.IgnoreBecauseCalculate(r => r.Instance));
    }
}