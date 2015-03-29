namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System.Security.Policy;
    using System.Web;
    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(JavaScriptSelector))]
    public class When_java_script_selector
    {
        It should_be_eval = () => Selector.JS.Eval("123")
                                          .ToString()
                                          .ShouldEqual("||javascript*123||");


        It should_be_call = () => Selector.JS.Call("Method", 5, "aws")
                                          .ToString()
                                          .ShouldEqual("||javascript*Method(5,'aws')||");
        
        It should_be_confirm = () => Selector.JS.Confirm("My message '")
                                          .ToString()
                                          .ShouldEqual("||javascript*confirm('My message \\'')||");

        It should_be_inc_432 = () => Selector.JS.Call("StringAsFormatByInc", Selector.JS.Eval("resultData.Name"))
                                             .ToString()
                                             .ShouldEqual("");

        //.ShouldEqual("||javascript*StringAsFormatByInc(||javascript*resultData.Name||)||");
    }
}