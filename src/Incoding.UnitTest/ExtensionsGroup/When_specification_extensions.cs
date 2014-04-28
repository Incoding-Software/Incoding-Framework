namespace Incoding.UnitTest.ExtensionsGroup
{
    #region << Using >>

    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using Incoding.Data;
    using Incoding.Extensions;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(SpecificationExtensions))]
    public class When_specification_extensions
    {
        #region Fake classes

        class FakeSpec : Specification<IEntity>
        {
            public override Expression<Func<IEntity, bool>> IsSatisfiedBy()
            {
                return r => r.Id != null;
            }
        }

        class Fake2Spec : Specification<IEntity>
        {
            public override Expression<Func<IEntity, bool>> IsSatisfiedBy()
            {
                return r => r.Id.ToString() == "3";
            }
        }

        #endregion

        #region Establish value

        static IQueryable<IEntity> fakeCollection;

        #endregion

        Establish establish = () =>
                                  {
                                      var entity = Pleasure.MockAsObject<IEntity>(mock => mock.SetupGet(r => r.Id).Returns("3"));
                                      var entity2 = Pleasure.MockAsObject<IEntity>(mock => mock.SetupGet(r => r.Id).Returns("123"));
                                      fakeCollection = Pleasure.ToQueryable(entity, entity2);
                                  };

        It should_be_and = () => fakeCollection
                                         .Count(new FakeSpec().And(new Fake2Spec()).IsSatisfiedBy())
                                         .ShouldEqual(1);

        It should_be_or = () => fakeCollection
                                        .Count(new FakeSpec().Or(new Fake2Spec()).IsSatisfiedBy())
                                        .ShouldEqual(2);
    }
}