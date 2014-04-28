namespace Incoding.UnitTest.ExtensionsGroup
{
    #region << Using >>

    using System.Collections.Generic;
    using System.Linq;
    using Incoding.Extensions;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(EnumerableExtensions))]
    public class When_enumerable_extensions_page_
    {
        #region Establish value

        static List<int> collection;

        static IQueryable<int> result;

        #endregion

        Establish establish = () => { collection = Pleasure.ToList(1, 5, 6, 2, 4, 7); };

        Because of = () => { result = collection.AsQueryable().Page(2, 3); };

        It should_be_verify = () => result.ShouldEqualWeakEach(new[] { 2, 4, 7 });
    }
}