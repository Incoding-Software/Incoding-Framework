namespace Incoding.UnitTest
{
    #region << Using >>

    using Incoding.Data;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(CollectionAsString))]
    public class When_collection_as_string_join_value
    {
        #region Estabilish value

        static string sourceCollection;

        #endregion

        Because of = () =>
                         {
                             sourceCollection = "item&item2";
                             CollectionAsString.Join(ref sourceCollection, "item3");
                         };

        It should_be_correct_string = () => sourceCollection.ShouldEqual("item&item2&item3&");
    }
}