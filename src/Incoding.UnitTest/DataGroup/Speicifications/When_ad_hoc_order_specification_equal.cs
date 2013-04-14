namespace Incoding.UnitTest
{
    #region << Using >>

    using Incoding.Data;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(AdHocOrderSpecification<>))]
    public class When_ad_hoc_order_specification_equal
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

        It should_be_null = () => new AdHocOrderSpecification<FakeEntity>().Equals(null).ShouldBeFalse();

        It should_be_different_count_expressions = () =>
                                                       {
                                                           var left = new AdHocOrderSpecification<FakeEntity>();
                                                           left.OrderBy(r => r.Id);
                                                           Catch
                                                                   .Exception(() => left.ShouldEqual(new AdHocOrderSpecification<FakeEntity>()))
                                                                   .ShouldBeOfType<SpecificationException>();
                                                       };

        It should_be_different_expressions = () =>
                                                 {
                                                     var left = new AdHocOrderSpecification<FakeEntity>();
                                                     left.OrderBy(r => r.Id);

                                                     var right = new AdHocOrderSpecification<FakeEntity>();
                                                     right.OrderBy(r => r.Prop);
                                                     Catch
                                                             .Exception(() => left.ShouldEqual(right))
                                                             .ShouldBeOfType<SpecificationException>();
                                                 };

        It should_be_different_type = () =>
                                          {
                                              var left = new AdHocOrderSpecification<FakeEntity>();
                                              left.OrderBy(r => r.Id);

                                              var right = new AdHocOrderSpecification<FakeEntity>();
                                              right.OrderByDescending(r => r.Id);
                                              Catch
                                                      .Exception(() => left.ShouldEqual(right))
                                                      .ShouldBeOfType<SpecificationException>();
                                          };
    }
}