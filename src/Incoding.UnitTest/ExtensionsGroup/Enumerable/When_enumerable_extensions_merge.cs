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
    public class When_enumerable_extensions_merge
    {
        #region Estabilish value

        static IEnumerable<string> collection;

        static IEnumerable<string> duplicate;

        static IEnumerable<string> result;

        #endregion

        Establish establish = () =>
                                  {
                                      collection = new List<string> { "V", "L", "A", "D" }.AsEnumerable();
                                      duplicate = new List<string> { "A", "D" }.AsEnumerable();
                                  };

        Because of = () => { result = collection.Merge(duplicate); };

        It should_be_remain_only_not_duplicate = () =>
                                                     {
                                                         result.FirstOrDefault(s => s.Equals("V", StringComparison.InvariantCultureIgnoreCase)).ShouldNotBeNull();
                                                         result.FirstOrDefault(s => s.Equals("L", StringComparison.InvariantCultureIgnoreCase)).ShouldNotBeNull();
                                                     };

        It should_be_remove_duplicate = () =>
                                            {
                                                result.FirstOrDefault(s => s.Equals("A", StringComparison.InvariantCultureIgnoreCase)).ShouldBeNull();
                                                result.FirstOrDefault(s => s.Equals("D", StringComparison.InvariantCultureIgnoreCase)).ShouldBeNull();
                                            };
    }
}