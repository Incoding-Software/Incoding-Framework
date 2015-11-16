namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Incoding.Extensions;
    using Incoding.MSpecContrib;
    using Incoding.MvcContrib;
    using Incoding.MvcContrib.MVD;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(IncodingSelector))]
    public class When_incoding_selector
    {
        #region Fake classes

        class FakeEmptyClass { }

        class FakeModel
        {
            #region Properties

            public string Prop { get; set; }

            #endregion
        }

        #endregion

        #region Establish value

        enum FakeEnum
        {
            Value = 5
        }

        static UrlDispatcher urlDispatcher;

        #endregion

        Establish establish = () =>
                                  {
                                      var routeData = new RouteData();
                                      routeData.Values.Add("Action", "Action");
                                      routeData.Values.Add("Controller", "Controller");

                                      var routes = new RouteCollection();
                                      routes.MapRoute(name: "Default",
                                                      url: "{controller}/{action}/{id}",
                                                      defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional });
                                      var httpContext = Pleasure.Mock<HttpContextBase>(mock =>
                                                                                           {
                                                                                               mock.Setup(r => r.Request.ApplicationPath).Returns("/");
                                                                                               mock.Setup(r => r.Response.ApplyAppPathModifier(Pleasure.MockIt.IsAny<string>())).Returns(Pleasure.Generator.TheSameString());
                                                                                           });
                                      var urlHelper = new UrlHelper(new RequestContext(httpContext.Object, routeData), routes);
                                      urlDispatcher = new UrlDispatcher(urlHelper);
                                  };

        It should_be_value = () => Selector
                                           .Value(Pleasure.Generator.TheSameString())
                                           .ToString()
                                           .ShouldEqual("TheSameString");

        It should_be_value_enum = () => Selector
                                                .Value(FakeEnum.Value)
                                                .ToString()
                                                .ShouldEqual("5");

        It should_be_hash = () => Selector.Incoding
                                          .HashUrl()
                                          .ToString()
                                          .ShouldEqual("||hashUrl*root||");

        It should_be_hash_with_prefix = () => Selector.Incoding
                                                      .HashUrl(prefix: "search")
                                                      .ToString()
                                                      .ShouldEqual("||hashUrl*search||");

        It should_be_query_string = () => Selector.Incoding
                                                  .QueryString<FakeModel>(r => r.Prop)
                                                  .ToString()
                                                  .ShouldEqual("||queryString*Prop||");

        It should_be_query_string_generic = () => MockHtmlHelper<FakeModel>
                                                          .When()
                                                          .Original
                                                          .Selector()
                                                          .QueryString(r => r.Prop)
                                                          .ToString()
                                                          .ShouldEqual("||queryString*Prop||");

        It should_be_hash_query_string_generic = () => Selector.Incoding
                                                               .HashQueryString<FakeModel>(r => r.Prop)
                                                               .ToString()
                                                               .ShouldEqual("||hashQueryString*Prop:root||");

        It should_be_hash_query_string_generic_helper = () => MockHtmlHelper<FakeModel>
                                                                      .When()
                                                                      .Original
                                                                      .Selector()
                                                                      .HashQueryString(r => r.Prop)
                                                                      .ToString()
                                                                      .ShouldEqual("||hashQueryString*Prop:root||");

        It should_be_hash_query_string = () => Selector.Incoding
                                                       .HashQueryString("Prop")
                                                       .ToString()
                                                       .ShouldEqual("||hashQueryString*Prop:root||");

        It should_be_hash_query_string_with_prefix = () => Selector.Incoding
                                                                   .HashQueryString<FakeModel>(r => r.Prop, prefix: "search")
                                                                   .ToString()
                                                                   .ShouldEqual("||hashQueryString*Prop:search||");

        It should_be_cookie = () => Selector.Incoding
                                            .Cookie("key")
                                            .ToString()
                                            .ShouldEqual("||cookie*key||");

        It should_be_cookie_generic = () => Selector.Incoding
                                                    .Cookie<FakeModel>(r => r.Prop)
                                                    .ToString()
                                                    .ShouldEqual("||cookie*Prop||");

        It should_be_cookie_generic_helper = () => MockHtmlHelper<FakeModel>
                                                           .When()
                                                           .Original
                                                           .Selector()
                                                           .Cookie(r => r.Prop)
                                                           .ToString()
                                                           .ShouldEqual("||cookie*Prop||");

        It should_be_ajax_get = () => Selector.Incoding
                                              .AjaxGet(Pleasure.Generator.Url().AppendToQueryString(new { id = "9847E8A2-6D73-450F-9BF6-075CB060E774".ToId() }))
                                              .ToString()
                                              .ShouldEqual("||ajax*{\"url\":\"http://sample.com\",\"type\":\"GET\",\"async\":false}||");

        It should_be_build_url = () => Selector.Incoding
                                               .BuildUrl("/Dispatcher/Query?type=IsQuery&id=123")
                                               .ToString()
                                               .ShouldEqual("||buildurl*/Dispatcher/Query?type=IsQuery&id=123||");

        It should_be_to_build_url = () => "/Dispatcher/Query?type=IsQuery&id=123"
                                                  .ToBuildUrl()
                                                  .ToString()
                                                  .ShouldEqual("||buildurl*/Dispatcher/Query?type=IsQuery&id=123||");

        It should_be_string_to_ajax_get = () => Pleasure.Generator.Url()
                                                        .ToAjaxGet()
                                                        .ToString()
                                                        .ShouldEqual("||ajax*{\"url\":\"http://sample.com\",\"type\":\"GET\",\"async\":false}||");

        It should_be_string_to_ajax = () => Pleasure.Generator.Url()
                                                    .ToAjax(options => options.Cache = true)
                                                    .ToString()
                                                    .ShouldEqual("||ajax*{\"url\":\"http://sample.com\",\"cache\":true,\"async\":false}||");

        It should_be_string_to_ajax_post = () => Pleasure.Generator.Url()
                                                         .ToAjaxPost()
                                                         .ToString()
                                                         .ShouldEqual("||ajax*{\"url\":\"http://sample.com\",\"type\":\"POST\",\"async\":false}||");

        It should_be_url_to_ajax = () => urlDispatcher.Push(new FakeEmptyClass())
                                                      .ToAjax(options => options.Cache = false)
                                                      .ToString()
                                                      .ShouldEqual("||ajax*{\"url\":\"TheSameString\",\"cache\":false,\"async\":false}||");

        It should_be_url_to_ajax_get = () => urlDispatcher.Push(new FakeEmptyClass())
                                                          .ToAjaxGet()
                                                          .ToString()
                                                          .ShouldEqual("||ajax*{\"url\":\"TheSameString\",\"type\":\"GET\",\"async\":false}||");

        It should_be_url_to_ajax_post = () => urlDispatcher.Push(new FakeEmptyClass())
                                                           .ToAjaxPost()
                                                           .ToString()
                                                           .ShouldEqual("||ajax*{\"url\":\"TheSameString\",\"type\":\"POST\",\"async\":false}||");

        
        It should_be_ajax_post = () => Selector.Incoding
                                               .AjaxPost(Pleasure.Generator.Url())
                                               .ToString()
                                               .ShouldEqual("||ajax*{\"url\":\"http://sample.com\",\"type\":\"POST\",\"async\":false}||");
    }
}