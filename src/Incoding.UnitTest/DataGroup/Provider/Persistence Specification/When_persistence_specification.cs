namespace Incoding.UnitTest.MSpecGroup
{
    #region << Using >>

    using System;
    using System.Security.Permissions;
    using Incoding.Data;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(PersistenceSpecification<>))]
    public class When_persistence_specification
    {
        #region Fake classes

        enum HardEnum
        {
            Test,
            Test2,
            Test3
        }

         class HardFakeEntity : IncEntityBase
        {
            public DateTime DateTime { get; set; }

            public int Int { get; set; }

            public HardEnum Hard { get; set; }

        }

        class FakeEntityWithOnlyGetProperty : IncEntityBase
        {
            #region Properties

            public string First { get { return Pleasure.Generator.TheSameString(); } }

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

        It should_be_can_not_ignore_and_check = () => Catch.Exception(() => new PersistenceSpecification<FakeEntity>(session)
                .IgnoreBecauseCalculate(r => r.First)
                .CheckProperty(r => r.First, Pleasure.Generator.String())
                .VerifyMappingAndSchema())
                .Should(exception => exception.Message.ShouldEqual("Fields:First was ignore and can't check"));

        It should_be_with_entity = () => Catch.Exception(() =>
                                                         {
                                                             var fakeEntity = Pleasure.Generator.Invent<FakeEntity>();
                                                             new PersistenceSpecification<FakeEntity>(Pleasure.MockAsObject<IRepository>(mock => mock.Setup(r => r.GetById<FakeEntity>(fakeEntity.Id)).Returns(fakeEntity)))
                                                                     .VerifyMappingAndSchema(setting => setting.WithEntity(fakeEntity));
                                                         })
                .ShouldBeNull();

        It should_be_invent = () =>
                              {
                                  var repository = Pleasure.Mock<IRepository>(mock => mock.Setup(r => r.GetById<FakeEntity>(Pleasure.MockIt.IsAny<object>())).Returns(new FakeEntity()));

                                  new PersistenceSpecification<FakeEntity>(repository.Object)
                                          .IgnoreBecauseCalculate(r => r.First)
                                          .IgnoreBecauseCalculate(r => r.Last)
                                          .VerifyMappingAndSchema();

                                  Action<FakeEntity> verify = entity =>
                                                              {
                                                                  entity.First.ShouldBeNull();
                                                                  entity.Last.ShouldBeNull();
                                                              };
                                  repository.Verify(r => r.Save(Pleasure.MockIt.Is(verify)));
                              };

        It should_be_equal = () =>
                             {
                                 var repository = Pleasure.Mock<IRepository>(mock => mock.Setup(r => r.GetById<FakeEntity>(Pleasure.MockIt.IsAny<object>())).Returns(new FakeEntity()));
                                 var ex = Catch.Exception(() => new PersistenceSpecification<FakeEntity>(repository.Object)
                                         .VerifyMappingAndSchema());
                                 ex.Message.ShouldContain("First with First");
                                 ex.Message.ShouldContain("Last with Last");
                             };

        It should_be_null_get_by_id = () =>
                                      {
                                          var repository = Pleasure.Mock<IRepository>();
                                          var ex = Catch.Exception(() => new PersistenceSpecification<FakeEntity>(repository.Object)
                                                  .VerifyMappingAndSchema());
                                          ex.Message.ShouldEqual("Can't found entity FakeEntity by id ");
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
                                                   .IgnoreBecauseCalculate(r => r.First)
                                                   .IgnoreBecauseCalculate(r => r.Last)
                                                   .VerifyMappingAndSchema();

                                           Action<FakeEntity> verify = entity =>
                                                                       {
                                                                           entity.First.ShouldBeNull();
                                                                           entity.Last.ShouldBeNull();
                                                                       };
                                           repository.Verify(r => r.Save(Pleasure.MockIt.Is(verify)));
                                       };

        It should_be_ignore_base_class = () =>
                                         {
                                             var db = new FakeChildEntity() { Middle = Pleasure.Generator.String() };
                                             var repository = Pleasure.Mock<IRepository>(mock => mock.Setup(r => r.GetById<FakeChildEntity>(Pleasure.MockIt.IsAny<object>())).Returns(db));

                                             new PersistenceSpecification<FakeChildEntity>(repository.Object)
                                                     .IgnoreBaseClass()
                                                     .CheckProperty(r => r.Middle, db.Middle)
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