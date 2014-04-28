namespace Incoding.UnitTest.ExtensionsGroup
{
    #region << Using >>

    using System.Collections.Generic;
    using System.Web.Routing;
    using Incoding.Extensions;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(AnonymousHelper))]
    public class When_route_value_dictionary_helper_from_route_value_dictionary
    {
        #region Establish value

        static RouteValueDictionary routeValueDictionary;

        static RouteValueDictionary result;

        #endregion

        Establish establish = () => { routeValueDictionary = new RouteValueDictionary(new Dictionary<string, object> { { "id", "2" }, { "value", "value" } }); };

        Because of = () => { result = AnonymousHelper.ToDictionary(routeValueDictionary); };

        It should_be_has_all_key = () =>
                                       {
                                           result.ShouldBeKeyValue("Id", "2");
                                           result.ShouldBeKeyValue("Value", "value");
                                       };
    }
}