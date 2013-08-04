namespace Incoding.UnitTest.MSpecGroup
{
    #region << Using >>

    using Incoding.Data;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(CompareFactory<FakeCompareDuplicate, FakeCompareDuplicate>))]
    public class When_compare_factory_with_duplication
    {
        #region Fake classes

        class FakeCompareDuplicate : IncEntityBase
        {
            #region Properties

            public new virtual int Id { get; set; }

            #endregion
        }

        #endregion

        It should_not_be_compare_class_with_duplicate_field = () =>
                                                                  {
                                                                      var compare = new CompareFactory<FakeCompareDuplicate, FakeCompareDuplicate>();
                                                                      compare.Compare(new FakeCompareDuplicate
                                                                                          {
                                                                                                  Id = Pleasure.Generator.PositiveNumber()
                                                                                          }, new FakeCompareDuplicate());
                                                                      compare.IsCompare().ShouldBeFalse();
                                                                  };

        It should_be_compare_class_with_duplicate_field = () =>
                                                              {
                                                                  var compare = new CompareFactory<FakeCompareDuplicate, FakeCompareDuplicate>();
                                                                  var fakeCompareDuplicate = new FakeCompareDuplicate
                                                                                                 {
                                                                                                         Id = Pleasure.Generator.PositiveNumber()
                                                                                                 };
                                                                  compare.Compare(fakeCompareDuplicate, fakeCompareDuplicate);
                                                                  compare.IsCompare().ShouldBeTrue();
                                                              };
    }
}