namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(Selector))]
    public class When_selector_add_method
    {
        It should_be_add_method_with_brakes = () => Selector.Jquery
                                                            .Id("test")
                                                            .Method("MyMethod()")
                                                            .ToString()
                                                            .ShouldEqual("$('#test').MyMethod()");

        It should_be_add_method_with_args = () => Selector.Jquery
                                                          .Id("test")
                                                          .Method("MyMethod", 1, "3")
                                                          .ToString()
                                                          .ShouldEqual("$('#test').MyMethod(1,'3')");

        It should_be_add_method_with_selector_args = () => Selector.Jquery
                                                                   .Id("test")
                                                                   .Method("MyMethod", Selector.Jquery.Id("id"))
                                                                   .ToString()
                                                                   .ShouldEqual("$('#test').MyMethod($('#id'))");

        It should_be_add_method_without_brakes = () => Selector.Jquery
                                                               .Id("test")
                                                               .Method("MyMethod")
                                                               .ToString()
                                                               .ShouldEqual("$('#test').MyMethod()");
    }
}