namespace Incoding.UnitTest
{
    #region << Using >>

    using Incoding.Data;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(DictionaryAsString))]
    public class When_dictionary_as_string_join
    {
        #region Establish value

        static string sourceCollection;

        #endregion

        Establish establish = () => { sourceCollection = "key=value"; };

        Because of = () => DictionaryAsString.Join(ref sourceCollection, "key2", "value2");

        It should_be_correct_string = () => sourceCollection.ShouldEqual("key=value&key2=value2&");
    }
}