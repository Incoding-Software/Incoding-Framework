namespace Incoding.UnitTest
{
    #region << Using >>

    using System.Collections.Generic;
    using Incoding.Data;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(DictionaryAsString))]
    public class When_dictionary_as_string_parse
    {
        #region Establish value

        static string sourceCollection;

        static IDictionary<string, string> dictionary;

        #endregion

        Establish establish = () => { sourceCollection = "key=value&key1=value1"; };

        Because of = () => { dictionary = DictionaryAsString.Parse(sourceCollection); };

        It should_be_has_elements = () => dictionary.Count.ShouldEqual(2);

        It should_be_has_pair_1 = () => dictionary.ShouldBeKeyValue("key", "value");

        It should_be_has_pair_2 = () => dictionary.ShouldBeKeyValue("key1", "value1");
    }
}