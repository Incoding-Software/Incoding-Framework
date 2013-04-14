namespace Incoding.UnitTest
{
    #region << Using >>

    using Incoding.Data;
    using Machine.Specifications;using Incoding.MSpecContrib;

    #endregion

    [Subject(typeof(DictionaryAsString))]
    public class When_dictionary_as_string_join_with_collections
    {
        #region Estabilish value

        static string strCollection;

        #endregion

        Establish establish = () => { strCollection = "key=value"; };

        Because of = () => DictionaryAsString.Join(ref strCollection, Pleasure.ToDynamicDictionary<string>(new { key2 = "value2", key3 = "value3" }));

        It should_be_valid_string = () => strCollection.ShouldEqual("key=value&key2=value2&key3=value3&");
    }
}