namespace Incoding.UnitTest.MSpecGroup
{
    #region << Using >>

    using Incoding.MSpecContrib;
    using Machine.Specifications;    

    #endregion

    [Subject(typeof(PersistenceSpecification<>))]
    public class When_persistence_specification_base_class_no_establish : SpecWithPersistenceSpecification<DbEntityReference>
    {
        It should_be_by_verify = () => persistenceSpecification
                                               .VerifyMappingAndSchema();
    }
}