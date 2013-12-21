namespace Incoding.UnitTest.MSpecGroup
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using Incoding.Data;
    using Incoding.Extensions;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(IncodingMoqExtensions))]
    public class When_incoding_moq_extensions_repository_stub_query
    {
        #region Fake classes

        class Spec : Specification<IEntity>
        {
            #region Fields

            readonly string id;

            #endregion

            #region Constructors

            public Spec(string id)
            {
                this.id = id;
            }

            #endregion

            ////ncrunch: no coverage start
            public override Expression<Func<IEntity, bool>> IsSatisfiedBy()
            {
                return null;
            }

            ////ncrunch: no coverage end        
        }

        class UserFacilitiesSpecification : Specification<IEntity>
        {
            readonly IEnumerable<Guid> _facilities;

            public UserFacilitiesSpecification(IEnumerable<Guid> facilities)
            {
                this._facilities = facilities;
            }

            public override Expression<Func<IEntity, bool>> IsSatisfiedBy()
            {
                return null;
            }
        }

        class SpecWithoutFields : Specification<IEntity>
        {
            ////ncrunch: no coverage start
            public override Expression<Func<IEntity, bool>> IsSatisfiedBy()
            {
                return null;
            }

            ////ncrunch: no coverage end        
        }

        #endregion

        It should_be_spec_without_fields = () =>
                                               {
                                                   var repository = Pleasure.Mock<IRepository>();

                                                   repository.StubQuery(whereSpecification: new SpecWithoutFields(), 
                                                                        entities: Pleasure.MockStrictAsObject<IEntity>(mock => mock.SetupGet(r => r.Id).Returns(Pleasure.Generator.TheSameString())));

                                                   repository.Object.Query(whereSpecification: new SpecWithoutFields())
                                                             .First()
                                                             .Id.ShouldEqual(Pleasure.Generator.TheSameString());
                                               };

        It should_be_spec_with_array = () =>
                                           {
                                               var repository = Pleasure.Mock<IRepository>();

                                               var ids = Pleasure.ToEnumerable(Guid.NewGuid(), Guid.NewGuid());
                                               repository.StubQuery(whereSpecification: new UserFacilitiesSpecification(ids), 
                                                                    entities: Pleasure.MockStrictAsObject<IEntity>(mock => mock.SetupGet(r => r.Id).Returns(Pleasure.Generator.TheSameString())));

                                               repository.Object.Query(whereSpecification: new UserFacilitiesSpecification(ids.ToArray()))
                                                         .First()
                                                         .ShouldNotBeNull();
                                           };



        It should_be_spec_with_array_and_next_spec = () =>
                                                         {
                                                             var repository = Pleasure.Mock<IRepository>();

                                                             var ids = Pleasure.ToEnumerable(Guid.NewGuid(), Guid.NewGuid());
                                                             repository.StubQuery(whereSpecification: new UserFacilitiesSpecification(ids)
                                                                                          .And(new SpecWithoutFields()),
                                                                                  entities: Pleasure.MockStrictAsObject<IEntity>(mock => mock.SetupGet(r => r.Id).Returns(Pleasure.Generator.TheSameString())));

                                                             repository.Object.Query(whereSpecification: new UserFacilitiesSpecification(ids)
                                                                                             .And(new SpecWithoutFields()))
                                                                       .First()
                                                                       .ShouldNotBeNull();
                                                         };

        It should_be_spec_with_array_and_next_spec_false = () =>
                                                               {
                                                                   var repository = Pleasure.Mock<IRepository>();

                                                                   var ids = Pleasure.ToEnumerable(Guid.NewGuid(), Guid.NewGuid());
                                                                   repository.StubQuery(whereSpecification: new UserFacilitiesSpecification(ids)
                                                                                                .And(new SpecWithoutFields()),
                                                                                        entities: Pleasure.MockStrictAsObject<IEntity>(mock => mock.SetupGet(r => r.Id).Returns(Pleasure.Generator.TheSameString())));

                                                                   repository.Object.Query(whereSpecification: new UserFacilitiesSpecification(ids))
                                                                             .ShouldBeEmpty();
                                                               };

        It should_be_spec_with_array_false = () =>
                                                 {
                                                     var repository = Pleasure.Mock<IRepository>();

                                                     repository.StubQuery(whereSpecification: new UserFacilitiesSpecification(Pleasure.ToEnumerable(Guid.NewGuid(), Guid.NewGuid())), 
                                                                          entities: Pleasure.MockStrictAsObject<IEntity>(mock => mock.SetupGet(r => r.Id).Returns(Pleasure.Generator.TheSameString())));

                                                     repository.Object.Query(whereSpecification: new UserFacilitiesSpecification(Pleasure.ToEnumerable(Guid.NewGuid(), Guid.NewGuid())))
                                                               .ShouldBeEmpty();
                                                 };

        It should_be_spec_without_fields_false = () =>
                                                     {
                                                         var repository = Pleasure.Mock<IRepository>();

                                                         repository.StubQuery(entities: Pleasure.MockStrictAsObject<IEntity>(mock => mock.SetupGet(r => r.Id).Returns(Pleasure.Generator.TheSameString())));
                                                         repository.Object.Query(whereSpecification: new SpecWithoutFields()).ShouldBeEmpty();
                                                     };

        It should_be_spec_without_fields_and_with = () =>
                                                        {
                                                            var repository = Pleasure.Mock<IRepository>();

                                                            repository.StubQuery(whereSpecification: new Spec(Pleasure.Generator.TheSameString())
                                                                                         .And(new SpecWithoutFields()), 
                                                                                 entities: Pleasure.MockStrictAsObject<IEntity>(mock => mock.SetupGet(r => r.Id).Returns(Pleasure.Generator.TheSameString())));

                                                            repository.Object.Query(whereSpecification: new Spec(Pleasure.Generator.TheSameString())
                                                                                            .And(new SpecWithoutFields()))
                                                                      .First()
                                                                      .Id.ShouldEqual(Pleasure.Generator.TheSameString());
                                                        };

        It should_be_entity_identity_with_different_fields = () =>
                                                                 {
                                                                     string entityId1 = Pleasure.Generator.String();
                                                                     string entityId2 = Pleasure.Generator.String();
                                                                     var repository = Pleasure.Mock<IRepository>(mock =>
                                                                                                                     {
                                                                                                                         mock.StubQuery(whereSpecification: new Spec(entityId1), 
                                                                                                                                        entities: Pleasure.MockStrictAsObject<IEntity>(s => s.SetupGet(r => r.Id).Returns(entityId1)));

                                                                                                                         mock.StubQuery(whereSpecification: new Spec(entityId2), 
                                                                                                                                        entities: Pleasure.MockStrictAsObject<IEntity>(s => s.SetupGet(r => r.Id).Returns(entityId2)));
                                                                                                                     });

                                                                     repository.Object.Query(whereSpecification: new Spec(entityId1))
                                                                               .First()
                                                                               .Id.ShouldEqual(entityId1);

                                                                     repository.Object.Query(whereSpecification: new Spec(entityId2))
                                                                               .First()
                                                                               .Id.ShouldEqual(entityId2);
                                                                 };
    }
}