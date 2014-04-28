namespace Incoding.UnitTest
{
    #region << Using >>

    using System.Collections.Generic;
    using System.Linq;
    using Incoding.Data;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(CollectionAsString))]
    public class When_collection_as_string_parse_source_without_split_char
    {
        #region Establish value

        static string strCollection;

        static string value1;

        static string value2;

        static IEnumerable<string> collection;

        #endregion

        Establish establish = () =>
                                  {
                                      value1 = Pleasure.Generator.String();
                                      strCollection = value1;
                                  };

        Because of = () => { collection = CollectionAsString.Parse(strCollection); };

        It should_be_load_from_string = () => collection.Count().ShouldEqual(1);

        It should_be_has_all_element = () => collection.Contains(value1).ShouldBeTrue();
    }
}