namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using Incoding.Extensions;
    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(JavaScriptSelector))]
    public class When_java_script_selector
    {
        #region Fake classes

        class HealthcareSystemProviderDetailsVm
        {
            #region Properties

            public object AdrVal1 { get; set; }

            #endregion
        }

        #endregion

        It should_be_eval = () => Selector.JS.Eval("123")
                                          .ToString()
                                          .ShouldEqual("||javascript*123||");

        It should_be_call = () => Selector.JS.Call("Method", 5, "aws")
                                          .ToString()
                                          .ShouldEqual("||javascript*Method(5,'aws')||");

        It should_be_call_with_selector = () => Selector.JS.Call("Method", Selector.JS.Call("FormatPhone", Selector.Result.For<HealthcareSystemProviderDetailsVm>(r => r.AdrVal1)))
                                                        .ToString()
                                                        .ShouldEqual("||javascript*Method(||javascript*FormatPhone)||");

        It should_be_confirm = () => Selector.JS.Confirm("My message '")
                                             .ToString()
                                             .ShouldEqual("||javascript*confirm('My message \\'')||");

        It should_be_confirm_extensions = () => "My message '".ToConfirm()
                                                              .ToString()
                                                              .ShouldEqual("||javascript*confirm('My message \\'')||");

        It should_be_inc_432 = () => Selector.JS.Call("StringAsFormatByInc", Selector.JS.Eval("resultData.Name"))
                                             .ToString()
                                             .ShouldEqual("||javascript*StringAsFormatByInc(||javascript*resultData.Name||)||");
    }
}