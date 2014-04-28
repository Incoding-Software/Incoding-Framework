namespace Incoding.UnitTest.ExtensionsGroup
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Incoding.Extensions;
    using Machine.Specifications;

    [Subject(typeof(EnumerableExtensions))]
    public class When_enumerable_extensions_merger
    {
        #region Establish value

        static IEnumerable<string> source;

        static IEnumerable<string> dest;

        static IEnumerable<string> result;

        #endregion

        Establish establish = () =>
                                  {
                                      source = new List<string> { "V", "L", "D" }.AsEnumerable();
                                      dest = new List<string> { "A", "D", "G" }.AsEnumerable();
                                  };

        Because of = () => { result = source.Merger(dest); };

        It should_be_remain_only_not_duplicate = () =>
                                                     {
                                                         result.Count().ShouldEqual(5);
                                                         foreach (var item in new[] { "V", "L", "D", "A", "G" })
                                                             result.Count(s => s.Equals(item, StringComparison.InvariantCultureIgnoreCase)).ShouldEqual(1);
                                                     };
    }
}