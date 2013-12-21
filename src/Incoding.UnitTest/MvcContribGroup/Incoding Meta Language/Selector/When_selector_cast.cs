namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System.Web.Mvc;
    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(Selector))]
    public class When_selector_cast
    {
        It should_be_cast_string_to_selector = () =>
                                                   {
                                                       const string str = "aws";
                                                       var selector = (Selector)str;

                                                       selector.ToString().ShouldEqual("aws");
                                                   };

        It should_be_cast_mvc_html_string_to_selector = () =>
                                                            {
                                                                var mvcHtmlString = new MvcHtmlString("aws");
                                                                var selector = (Selector)mvcHtmlString;

                                                                selector.ToString().ShouldEqual("aws");
                                                            };

        It should_be_cast_string_empty_to_selector = () =>
                                                         {
                                                             var selector = (Selector)string.Empty;

                                                             selector.ToString().ShouldEqual(string.Empty);
                                                         };

        It should_be_cast_int_to_selector = () =>
                                                {
                                                    const int value = 5;
                                                    var selector = (Selector)value;

                                                    selector.ToString().ShouldEqual("5");
                                                };

        It should_be_cast_object_int_to_selector = () =>
                                                       {
                                                           object value = 5;
                                                           var selector = Selector.FromObject(value);

                                                           selector.ToString().ShouldEqual("5");
                                                       };

        It should_be_cast_object_selector_to_selector = () =>
                                                            {
                                                                object value = Selector.Jquery.Id("id");
                                                                var selector = Selector.FromObject(value);

                                                                selector.ToString().ShouldEqual("$('#id')");
                                                            };

        It should_be_cast_long_to_selector = () =>
                                                 {
                                                     const long value = 5;
                                                     var selector = (Selector)value;

                                                     selector.ToString().ShouldEqual("5");
                                                 };

        It should_be_cast_decimal_to_selector = () =>
                                                    {
                                                        const decimal value = 5;
                                                        var selector = (Selector)value;

                                                        selector.ToString().ShouldEqual("5");
                                                    };

        It should_be_cast_float_to_selector = () =>
                                                  {
                                                      const float value = 5;
                                                      var selector = (Selector)value;

                                                      selector.ToString().ShouldEqual("5");
                                                  };

        It should_be_cast_bool_to_selector = () =>
                                                 {
                                                     const bool value = true;
                                                     var selector = (Selector)value;

                                                     selector.ToString().ShouldEqual("True");
                                                 };

        It should_be_cast_selector_to_string = () =>
                                                   {
                                                       var selector = Selector.Jquery.Self();
                                                       string str = selector;
                                                       selector.ToString().ShouldEqual("$(this.self)");
                                                   };
    }
}