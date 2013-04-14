namespace Incoding.UnitTest
{
    #region << Using >>

    using System.Collections.Generic;
    using System.Linq;
    using Incoding.Data;
    using Machine.Specifications;using Incoding.MSpecContrib;

    #endregion

    [Subject(typeof(CollectionAsString))]
    public class When_collection_as_string_parse_empty_source
    {
        #region Estabilish value

        static string strCollection;

        static IList<string> collection;

        #endregion

        Establish establish = () => { strCollection = string.Empty; };

        Because of = () => { collection = CollectionAsString.Parse(strCollection).ToList(); };

        It should_be_not_be_null = () => collection.ShouldNotBeNull();

        It should_be_empty = () => collection.ShouldBeEmpty();
    }
}