namespace Incoding.UnitTest.ExtensionsGroup
{
    #region << Using >>

    using Incoding.Extensions;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(EnumerableExtensions))]
    public class When_enumerable_extension
    {
        It should_be_page = () => Pleasure
                                          .ToList(1, 5, 6, 2, 4, 7)
                                          .Page(2, 3)
                                          .ShouldEqualWeakEach(new[] { 2, 4, 7 });

        It should_be_page_with_current_0 = () => Pleasure
                                                         .ToList(1, 5, 6, 2, 4, 7)
                                                         .Page(0, 3)
                                                         .ShouldEqualWeakEach(new[] { 1, 5, 6 });

        It should_be_page_queryable = () => Pleasure
                                                    .ToQueryable(1, 5, 6, 2, 4, 7)
                                                    .Page(2, 3)
                                                    .ShouldEqualWeakEach(new[] { 2, 4, 7 });
    }
}