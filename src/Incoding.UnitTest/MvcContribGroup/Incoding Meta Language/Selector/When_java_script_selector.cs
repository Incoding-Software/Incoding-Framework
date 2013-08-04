namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(JavaScriptSelector))]
    public class When_java_script_selector
    {
        It should_be_eval = () => Selector.JS.Eval("123")
                                          .ToString()
                                          .ShouldEqual("@@javascript:123@@");
    }
}