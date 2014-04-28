namespace Incoding.UnitTest
{
    #region << Using >>

    using System.Web;
    using Incoding.Data;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(CollectionAsString))]
    public class When_collection_as_string_join_value_in_empty_collection
    {
        #region Establish value

        static string value;

        static string sourceCollection;

        #endregion

        Establish establish = () => { value = Pleasure.Generator.String(); };

        Because of = () =>
                         {
                             sourceCollection = string.Empty;
                             CollectionAsString.Join(ref sourceCollection, value);
                         };

        It should_be_correct_string = () => sourceCollection.ShouldEqual(HttpUtility.UrlEncode(value) + "&");
    }
}