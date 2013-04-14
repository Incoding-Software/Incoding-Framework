namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using Incoding.Extensions;
    using Incoding.MSpecContrib;
    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(IncodingSelector))]
    public class When_incoding_selector
    {
        #region Fake classes

        class FakeModel
        {
            #region Properties

            public string Prop { get; set; }

            #endregion
        }

        #endregion

        It should_be_value = () => Selector
                                           .Value(Pleasure.Generator.TheSameString())
                                           .ToString()
                                           .ShouldEqual("'TheSameString'");

        It should_be_hash = () => Selector.Incoding
                                          .HashUrl()
                                          .ToString()
                                          .ShouldEqual("'@@@@root@@@@'");

        It should_be_hash_with_prefix = () => Selector.Incoding
                                                      .HashUrl(prefix: "search")
                                                      .ToString()
                                                      .ShouldEqual("'@@@@search@@@@'");

        It should_be_href = () => Selector.Incoding
                                          .Href()
                                          .ToString()
                                          .ShouldEqual("'@@href@@'");

        It should_be_query_string = () => Selector.Incoding
                                                  .QueryString<FakeModel>(r => r.Prop)
                                                  .ToString()
                                                  .ShouldEqual("'@@@Prop@@@'");

        It should_be_hash_query_string_generic = () => Selector.Incoding
                                                               .HashQueryString<FakeModel>(r => r.Prop)
                                                               .ToString()
                                                               .ShouldEqual("'@@@@@Prop:root@@@@@'");

        It should_be_hash_query_string = () => Selector.Incoding
                                                       .HashQueryString("Prop")
                                                       .ToString()
                                                       .ShouldEqual("'@@@@@Prop:root@@@@@'");

        It should_be_hash_query_string_with_prefix = () => Selector.Incoding
                                                                   .HashQueryString<FakeModel>(r => r.Prop, prefix: "search")
                                                                   .ToString()
                                                                   .ShouldEqual("'@@@@@Prop:search@@@@@'");

        It should_be_cookie = () => Selector.Incoding
                                            .Cookie("key")
                                            .ToString()
                                            .ShouldEqual("'@@@@@@key@@@@@@'");

        It should_be_cookie_generic = () => Selector.Incoding
                                                    .Cookie<FakeModel>(r => r.Prop)
                                                    .ToString()
                                                    .ShouldEqual("'@@@@@@Prop@@@@@@'");

        It should_be_ajax_get = () => Selector.Incoding
                                              .AjaxGet(Pleasure.Generator.Url())
                                              .ToString()
                                              .ShouldEqual("'@@@@@@@{\"url\":\"http://sample.com\",\"type\":\"GET\",\"async\":false}@@@@@@@'");

        It should_be_ajax_post = () => Selector.Incoding
                                               .AjaxPost(Pleasure.Generator.Url())
                                               .ToString()
                                               .ShouldEqual("'@@@@@@@{\"url\":\"http://sample.com\",\"type\":\"POST\",\"async\":false}@@@@@@@'");
    }
}