namespace Incoding.UnitTest
{
    #region << Using >>

    using Incoding.Data;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(AdHocFetchNhibernateSpecification<>))]
    public class When_ad_hoc_fetch_specification_equal
    {
        #region Fake classes

        class FakeEntity : IEntity
        {
            #region Properties

            public string Prop { get; set; }

            #endregion

            #region IEntity Members

            public object Id { get; private set; }

            #endregion
        }

        #endregion

        It should_be_null = () => new AdHocFetchNhibernateSpecification<FakeEntity>().Equals(null).ShouldBeFalse();

        It should_be_different_count_expressions = () =>
                                                       {
                                                           var left = new AdHocFetchNhibernateSpecification<FakeEntity>();
                                                           left.Join(r => r.Prop);
                                                           Catch
                                                                   .Exception(() => left.ShouldEqual(new AdHocFetchNhibernateSpecification<FakeEntity>()))
                                                                   .ShouldBeAssignableTo<SpecificationException>();
                                                       };

        It should_be_different_expressions = () =>
                                                 {
                                                     var left = new AdHocFetchNhibernateSpecification<FakeEntity>();
                                                     left.Join(r => r.Prop);
                                                     var right = new AdHocFetchNhibernateSpecification<FakeEntity>();
                                                     right.Join(r => r.Id);
                                                     Catch
                                                             .Exception(() => left.ShouldEqual(right))
                                                             .ShouldBeAssignableTo<SpecificationException>();
                                                 };
    }
}