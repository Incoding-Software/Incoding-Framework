namespace Incoding.UnitTest.MSpecGroup
{
    #region << Using >>

    using FluentNHibernate.Testing;
    using Incoding.Data;
    using Incoding.MSpecContrib;
    using Machine.Specifications;
    using NHibernate;

    #endregion

    [Subject(typeof(NhibernatePersistenceSpecificationExtensions))]
    public class When_nhibernate_persistence_specification
    {
        #region Fake classes

        class FakeEntity : IncEntityBase
        {
            #region Properties

            public string First { get; set; }

            public string Last { get; set; }

            #endregion
        }

        class FakeChildEntity : FakeEntity
        {
            #region Properties

            public string Middle { get; set; }

            #endregion
        }

        #endregion

        #region Establish value

        static ISession session = Pleasure.MockAsObject<ISession>();

        #endregion

        It should_be_duplicate_property = () => Catch.Exception(() => new PersistenceSpecification<FakeEntity>(session)
                                                                              .CheckProperty(r => r.First, Pleasure.Generator.String())
                                                                              .CheckProperty(r => r.First, Pleasure.Generator.String())
                                                                              .VerifyMappingAndSchema())
                                                     .Should(exception => exception.Message.ShouldEqual("Duplicate fields: First"));

        It should_be_forget_property = () => Catch.Exception(() => new PersistenceSpecification<FakeEntity>(session).VerifyMappingAndSchema())
                                                  .Should(exception => exception.Message.ShouldEqual("Not found fields: First,Last"));

        It should_be_ignore_property = () => Catch.Exception(() => new PersistenceSpecification<FakeEntity>(session)
                                                                           .VerifyMappingAndSchema(setting => setting.IgnoreBecauseCalculate(r => r.First)))
                                                  .Should(exception => exception.Message.ShouldEqual("Not found fields: Last"));

        It should_be_ignore_base_class = () => Catch.Exception(() => new PersistenceSpecification<FakeChildEntity>(session)
                                                                             .VerifyMappingAndSchema(setting => setting.IgnoreBaseClass()))
                                                    .Should(exception => exception.Message.ShouldEqual("Not found fields: Middle"));
    }
}