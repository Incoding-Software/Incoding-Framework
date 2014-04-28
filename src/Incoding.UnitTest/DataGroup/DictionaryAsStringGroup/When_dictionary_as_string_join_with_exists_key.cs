namespace Incoding.UnitTest
{
    #region << Using >>

    using Incoding.Data;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(DictionaryAsString))]
    public class When_dictionary_as_string_join_with_exists_key
    {
        #region Establish value

        static string sourceCollection;

        #endregion

        Establish establish = () => { sourceCollection = "key=value"; };

        Because of = () => DictionaryAsString.Join(ref sourceCollection, "key", "newValue");

        It should_be_correct_string = () => sourceCollection.ShouldEqual("key=newValue&");
    }
}