namespace Incoding.UnitTest.MSpecGroup
{
    #region << Using >>

    using Incoding.MSpecContrib;
    using Machine.Specifications;    

    #endregion

    [Subject(typeof(PersistenceSpecification<>))]
    public class When_persistence_specification_with_base_class : SpecWithPersistenceSpecification<DbEntity>
    {
        It should_be_by_verify = () => persistenceSpecification
                                               .CheckProperty(r => r.Value, Pleasure.Generator.String())
                                               .CheckProperty(r => r.ValueNullable, Pleasure.Generator.PositiveNumber())
                                               .CheckProperty(r => r.Reference)
                                               .CheckProperty(r => r.Items, Pleasure.ToList(Pleasure.Generator.Invent<DbEntityItem>()), (entity, itemEntity) => entity.AddItem(itemEntity))
                                               .VerifyMappingAndSchema();
    }
}