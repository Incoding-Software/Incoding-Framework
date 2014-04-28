namespace Incoding.UnitTest.ExtensionsGroup
{
    #region << Using >>

    using System.Web.Routing;
    using Incoding.Extensions;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(AnonymousHelper))]
    public class When_route_value_dictionary_helper_from_object
    {
        #region Establish value

        static RouteValueDictionary result;

        static object anonymousDictionary;

        static int value;

        #endregion

        Establish establish = () =>
                                  {
                                      value = Pleasure.Generator.PositiveNumber();
                                      anonymousDictionary = new
                                                                {
                                                                        StrValue = value.ToString(), IntValue = value
                                                                };
                                  };

        Because of = () => { result = AnonymousHelper.ToDictionary(anonymousDictionary); };

        It should_be_result = () =>
                                  {
                                      result.ShouldBeKeyValue("StrValue", value.ToString());
                                      result.ShouldBeKeyValue("IntValue", value);
                                  };
    }
}