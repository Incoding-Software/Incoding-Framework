namespace Incoding.UnitTest.ExtensionsGroup
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Incoding.Extensions;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(EnumerableExtensions))]
    public class When_enumerable_extensions_unique
    {
        #region Establish value

        static IEnumerable<string> source;

        static IEnumerable<string> dest;

        static IEnumerable<string> result;

        #endregion

        Establish establish = () =>
                                  {
                                      source = new List<string> { "V", "L", "A", "D" }.AsEnumerable();
                                      dest = new List<string> { "A", "D" }.AsEnumerable();
                                  };

        Because of = () => { result = source.Unique(dest); };

        It should_be_remain_only_not_duplicate = () =>
                                                     {
                                                         result.Count().ShouldEqual(2);
                                                         result.FirstOrDefault(s => s.Equals("V", StringComparison.InvariantCultureIgnoreCase)).ShouldNotBeNull();
                                                         result.FirstOrDefault(s => s.Equals("L", StringComparison.InvariantCultureIgnoreCase)).ShouldNotBeNull();
                                                     };
    }
}