namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System.Web.Routing;
    using Incoding.Extensions;
    using Incoding.MSpecContrib;
    using Incoding.MvcContrib;
    using Machine.Specifications;
    using Newtonsoft.Json.Linq;

    #endregion

    [Subject(typeof(IncodingMetaContainer))]
    public class When_incoding_meta_container_to_html_attributes
    {
        #region Estabilish value

        static void Verify(RouteValueDictionary route)
        {
            route.Count.ShouldEqual(2);
            route.GetOrDefault("class").ShouldEqual("class");
            var arrayMeta = (JContainer)route.GetOrDefault("incoding").ToString().DeserializeFromJson<object>();

            arrayMeta[0].Should(token =>
                                    {
                                        dynamic o = token;
                                        (o.type.Value as string).ShouldEqual("ExecutableDirectAction");
                                    });

            arrayMeta[1].Should(token =>
                                    {
                                        dynamic o = token;
                                        (o.type.Value as string).ShouldEqual("ExecutableRedirect");
                                    });
        }

        static IncodingMetaContainer incodingMetaContainer;

        #endregion

        Establish establish = () =>
                                  {
                                      incodingMetaContainer = new IncodingMetaContainer();
                                      incodingMetaContainer.Add(new ExecutableDirectAction(Pleasure.Generator.String()));
                                      incodingMetaContainer.Add(new ExecutableRedirect("redirectTo"));
                                  };

        It should_be_with_anonymous_attributes = () =>
                                                     {
                                                         var result = incodingMetaContainer.AsHtmlAttributes(new { @class = "class" });
                                                         Verify(result);
                                                     };

        It should_be_with_strong_attributes = () =>
                                                  {
                                                      var result = incodingMetaContainer.AsHtmlAttributes(new RouteValueDictionary { { "class", "class" } });
                                                      Verify(result);
                                                  };
    }
}