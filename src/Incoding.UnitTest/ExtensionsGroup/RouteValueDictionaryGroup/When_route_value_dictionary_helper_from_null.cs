namespace Incoding.UnitTest.ExtensionsGroup
{
    #region << Using >>

    using System.Web.Routing;
    using Incoding.Extensions;
    using Machine.Specifications;using Incoding.MSpecContrib;

    #endregion

    [Subject(typeof(AnonymousHelper))]
    public class When_route_value_dictionary_helper_from_null
    {
        #region Estabilish value

        static RouteValueDictionary result;

        #endregion

        Because of = () => { result = AnonymousHelper.ToDictionary(null); };

        It should_be_empty = () => result.Count.ShouldEqual(0);
    }
}