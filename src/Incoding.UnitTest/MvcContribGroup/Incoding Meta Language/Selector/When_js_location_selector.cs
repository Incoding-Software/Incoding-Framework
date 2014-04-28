namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(JSLocationSelector))]
    public class When_js_location_selector
    {
        #region Establish value

        static JSLocationSelector location;

        #endregion

        Establish establish = () => { location = Selector.JS.Location; };

        It should_be_host = () => location.Host
                                          .ToString()
                                          .ShouldEqual("||javascript*window.location.host||");

        It should_be_hash = () => location.Hash
                                          .ToString()
                                          .ShouldEqual("||javascript*window.location.hash||");

        It should_be_href = () => location.Href
                                          .ToString()
                                          .ShouldEqual("||javascript*window.location.href||");

        It should_be_host_name = () => location.HostName
                                               .ToString()
                                               .ShouldEqual("||javascript*window.location.hostname||");

        It should_be_path_name = () => location.PathName
                                               .ToString()
                                               .ShouldEqual("||javascript*window.location.pathname||");

        It should_be_port = () => location.Port
                                          .ToString()
                                          .ShouldEqual("||javascript*window.location.port||");

        It should_be_protocol = () => location.Protocol
                                              .ToString()
                                              .ShouldEqual("||javascript*window.location.protocol||");

        It should_be_search = () => location.Search
                                            .ToString()
                                            .ShouldEqual("||javascript*window.location.search||");
    }
}