using System.Linq;

namespace Incoding.UnitTest.MSpecGroup
{
    #region << Using >>

    using Incoding.MSpecContrib;
    using Machine.Specifications;    

    #endregion

    [Subject(typeof(PersistenceSpecification<>))]
    public class When_persistence_specification_with_base_class_no_establish : SpecWithPersistenceSpecification<DbEntity>
    {
        Establish establish = () =>
                              {
                                  dbEntityReference = new DbEntityReference();
                                  SpecWithRepository.Repository.Save(dbEntityReference);
                              };

        It should_be_by_verify = () => persistenceSpecification
                                               .CheckProperty(r => r.Items, Pleasure.ToList(Pleasure.Generator.Invent<DbEntityItem>()), (entity, itemEntity) => entity.AddItem(itemEntity))
                                               .VerifyMappingAndSchema();

        static DbEntityReference dbEntityReference;
    }
}