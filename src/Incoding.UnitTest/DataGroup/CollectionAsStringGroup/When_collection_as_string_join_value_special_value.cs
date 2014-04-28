namespace Incoding.UnitTest
{
    #region << Using >>

    using Incoding.Data;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(CollectionAsString))]
    public class When_collection_as_string_join_value_special_value
    {
        #region Establish value

        static string sourceCollection;

        #endregion

        Because of = () =>
                         {
                             sourceCollection = "item&item2";
                             CollectionAsString.Join(ref sourceCollection, "&&&&");
                         };

        It should_be_correct_string = () => sourceCollection.ShouldEqual("item&item2&%26%26%26%26&");
    }
}