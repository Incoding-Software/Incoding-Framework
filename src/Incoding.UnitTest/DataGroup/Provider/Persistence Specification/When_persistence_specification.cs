namespace Incoding.UnitTest.MSpecGroup
{
    #region << Using >>

    using System;
    using Incoding.Data;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(PersistenceSpecification<>))]
    public class When_persistence_specification
    {
        #region Fake classes

        class FakeEntityWithOnlyGetProperty : IncEntityBase
        {
            #region Properties

            public string First
            {
                get
                {
                    return
                            Pleasure.Generator.TheSameString();
                }
            }

            #endregion
        }

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

        static IRepository session = Pleasure.MockAsObject<IRepository>();

        #endregion

        It should_be_duplicate_property = () => Catch.Exception(() => new PersistenceSpecification<FakeEntity>(session)
                                                                              .CheckProperty(r => r.First, Pleasure.Generator.String())
                                                                              .CheckProperty(r => r.First, Pleasure.Generator.String())
                                                                              .VerifyMappingAndSchema())
                                                     .Should(exception => exception.Message.ShouldEqual("Duplicate fields:First"));

        It should_be_with_entity = () => Catch.Exception(() =>
                                                             {
                                                                 var fakeEntity = Pleasure.Generator.Invent<FakeEntity>();
                                                                 new PersistenceSpecification<FakeEntity>(Pleasure.MockAsObject<IRepository>(mock => mock.Setup(r => r.GetById<FakeEntity>(fakeEntity.Id)).Returns(fakeEntity)))
                                                                         .VerifyMappingAndSchema(setting => setting.WithEntity(fakeEntity));
                                                             })
                                              .ShouldBeNull();

        It should_be_auto = () =>
                                {
                                    var repository = Pleasure.Mock<IRepository>(mock => mock.Setup(r => r.GetById<FakeEntity>(Pleasure.MockIt.IsAny<object>())).Returns(new FakeEntity()));

                                    new PersistenceSpecification<FakeEntity>(repository.Object)
                                            .Comparator(dsl => dsl.IgnoreBecauseNotUse(r => r.First)
                                                                  .IgnoreBecauseNotUse(r => r.Last))
                                            .VerifyMappingAndSchema();

                                    Action<FakeEntity> verify = entity =>
                                                                    {
                                                                        entity.First.ShouldNotBeEmpty();
                                                                        entity.Last.ShouldNotBeEmpty();
                                                                    };
                                    repository.Verify(r => r.Save(Pleasure.MockIt.Is(verify)));
                                };       
        

        It should_be_only_get_property = () =>
                                             {
                                                 var repository = Pleasure.Mock<IRepository>(mock => mock.Setup(r => r.GetById<FakeEntityWithOnlyGetProperty>(Pleasure.MockIt.IsAny<object>())).Returns(new FakeEntityWithOnlyGetProperty()));

                                                 new PersistenceSpecification<FakeEntityWithOnlyGetProperty>(repository.Object)
                                                         .VerifyMappingAndSchema();

                                                 Action<FakeEntityWithOnlyGetProperty> verify = entity => entity.First.ShouldBeTheSameString();
                                                 repository.Verify(r => r.Save(Pleasure.MockIt.Is(verify)));
                                             };

        It should_be_ignore_property = () =>
                                           {
                                               var repository = Pleasure.Mock<IRepository>(mock => mock.Setup(r => r.GetById<FakeEntity>(Pleasure.MockIt.IsAny<object>())).Returns(new FakeEntity()));

                                               new PersistenceSpecification<FakeEntity>(repository.Object)
                                                       .Comparator(dsl => dsl.IgnoreBecauseNotUse(r => r.Last))
                                                       .IgnoreBecauseCalculate(r => r.First)
                                                       .VerifyMappingAndSchema();

                                               repository.Verify(r => r.Save(Pleasure.MockIt.Is<FakeEntity>(entity => entity.First.ShouldBeNull())));
                                           };

        It should_be_ignore_base_class = () =>
                                             {
                                                 var repository = Pleasure.Mock<IRepository>(mock => mock.Setup(r => r.GetById<FakeChildEntity>(Pleasure.MockIt.IsAny<object>())).Returns(new FakeChildEntity()));

                                                 new PersistenceSpecification<FakeChildEntity>(repository.Object)
                                                         .Comparator(dsl => dsl.IgnoreBecauseNotUse(r => r.Middle))
                                                         .IgnoreBaseClass()
                                                         .VerifyMappingAndSchema();

                                                 Action<FakeChildEntity> verify = entity =>
                                                                                      {
                                                                                          entity.First.ShouldBeNull();
                                                                                          entity.Last.ShouldBeNull();
                                                                                          entity.Middle.ShouldNotBeEmpty();
                                                                                      };
                                                 repository.Verify(r => r.Save(Pleasure.MockIt.Is(verify)));
                                             };
    }
}