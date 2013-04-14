namespace Incoding.UnitTest.ExtensionsGroup
{
    #region << Using >>

    using System.Collections.Generic;
    using System.Web.Routing;
    using Incoding.Extensions;
    using Machine.Specifications;using Incoding.MSpecContrib;

    #endregion

    [Subject(typeof(AnonymousHelper))]
    public class When_route_value_dictionary_helper_from_dictionary
    {
        #region Estabilish value

        static RouteValueDictionary result;

        static Dictionary<string, object> dictionary;

        #endregion

        Establish establish = () => { dictionary = new Dictionary<string, object> { { "id", "2" }, { "value", "value" } }; };

        Because of = () => { result = AnonymousHelper.ToDictionary(dictionary); };

        It should_be_has_all_key = () => result.Should(valueDictionary =>
                                                           {
                                                               valueDictionary.ShouldBeKeyValue("Id", "2");
                                                               valueDictionary.ShouldBeKeyValue("Value", "value");
                                                           });
    }
}