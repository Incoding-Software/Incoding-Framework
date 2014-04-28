namespace Incoding.UnitTest
{
    #region << Using >>

    using System.Collections.Generic;
    using Incoding.Data;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(DictionaryAsString))]
    public class When_dictionary_as_string_parse_with_null_source
    {
        #region Establish value

        static string sourceCollection;

        static IDictionary<string, string> dictionary;

        #endregion

        Establish establish = () => { sourceCollection = null; };

        Because of = () => { dictionary = DictionaryAsString.Parse(sourceCollection); };

        It should_be_not_null = () => dictionary.ShouldNotBeNull();

        It should_be_empty = () => dictionary.ShouldBeEmpty();
    }
}