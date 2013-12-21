namespace Incoding.UnitTest.MSpecGroup
{
    #region << Using >>

    using FluentNHibernate.Testing;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(SpecWithPersistenceSpecification<>))]
    public class When_spec_with_persistence_specification : SpecWithPersistenceSpecification<RealDbEntity>
    {
        Because of = () => persistenceSpecification
                                   .CheckProperty(r => r.Value, Pleasure.Generator.PositiveNumber())
                                   .CheckProperty(r => r.Value2, Pleasure.Generator.PositiveNumber())
                                   .CheckProperty(r => r.ValueStr, Pleasure.Generator.String())
                                   .CheckProperty(r => r.ValueNullable, Pleasure.Generator.PositiveNumber())
                                   .CheckReference(r => r.Reference, Pleasure.Generator.Invent<RealDbItemEntity>())
                                   .CheckList(r => r.Items, Pleasure.ToList(Pleasure.Generator.Invent<RealDbItemEntity>()), (entity, item) => entity.AddItem(item));

        It should_be_verify = () => persistenceSpecification.VerifyMappingAndSchema();
    }
}