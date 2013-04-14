namespace Incoding.UnitTest
{
    #region << Using >>

    using System.Collections.Generic;
    using System.Linq;
    using Incoding.Data;
    using Machine.Specifications;using Incoding.MSpecContrib;

    #endregion

    [Subject(typeof(CollectionAsString))]
    public class When_collection_as_string_parse_special_source
    {
        #region Estabilish value

        static string strCollection;

        static IEnumerable<string> collection;

        #endregion

        Establish establish = () => { strCollection = "item&item2&%26%26%26%26&"; };

        Because of = () => { collection = CollectionAsString.Parse(strCollection); };

        It should_be_special_value = () => collection.Contains("&&&&").ShouldBeTrue();
    }
}