namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(Selector))]
    public class When_selector
    {
        It should_be_add_method_with_braket = () =>
                                                  {
                                                      var selector = new Selector("selector");
                                                      selector.AddMethod("MyMethod()");
                                                      selector.ToString().ShouldEqual("'selector.MyMethod()'");
                                                  };

        It should_be_add_method_with_args = () =>
                                                {
                                                    var selector = new Selector("selector");
                                                    selector.AddMethod("MyMethod", 1, "2");
                                                    selector.ToString().ShouldEqual("'selector.MyMethod('1','2')'");
                                                };

        It should_be_add_method_without_braket = () =>
                                                     {
                                                         var selector = new Selector("selector");
                                                         selector.AddMethod("MyMethod");
                                                         selector.ToString().ShouldEqual("'selector.MyMethod()'");
                                                     };
    }
}