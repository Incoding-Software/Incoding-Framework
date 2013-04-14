namespace Incoding.UnitTest
{
    #region << Using >>

    using System.Collections.Generic;
    using System.Linq;
    using Incoding.Data;
    using Machine.Specifications;using Incoding.MSpecContrib;

    #endregion

    [Subject(typeof(CollectionAsString))]
    public class When_collection_as_string_parse_source
    {
        #region Estabilish value

        static string strCollection;

        static string value1;

        static string value2;

        static IEnumerable<string> collection;

        #endregion

        Establish establish = () =>
                                  {
                                      value1 = Pleasure.Generator.String();
                                      value2 = Pleasure.Generator.String();
                                      strCollection = string.Join("&", Pleasure.ToEnumerable(value1, value2));
                                  };

        Because of = () => { collection = CollectionAsString.Parse(strCollection); };

        It should_be_load_from_string = () => collection.Count().ShouldEqual(2);

        It should_be_has_all_element = () =>
                                           {
                                               collection.Contains(value1).ShouldBeTrue();
                                               collection.Contains(value2).ShouldBeTrue();
                                           };
    }
}