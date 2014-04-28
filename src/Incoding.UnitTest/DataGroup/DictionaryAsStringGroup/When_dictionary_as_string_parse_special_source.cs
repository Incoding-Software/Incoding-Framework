namespace Incoding.UnitTest
{
    #region << Using >>

    using System.Collections.Generic;
    using Incoding.Data;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(DictionaryAsString))]
    public class When_dictionary_as_string_parse_special_source
    {
        #region Establish value

        static string specialCollection;

        static IDictionary<string, string> dictionary;

        #endregion

        Establish establish = () => { specialCollection = "key=value&key2=special%3dspecial2%26&"; };

        Because of = () => { dictionary = DictionaryAsString.Parse(specialCollection); };

        It should_be_has_elements = () => dictionary.Count.ShouldEqual(2);

        It should_be_has_pair_1 = () => dictionary.ShouldBeKeyValue("key", "value");

        It should_be_has_pair_2 = () => dictionary.ShouldBeKeyValue("key2", "special=special2&");
    }
}