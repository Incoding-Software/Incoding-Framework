namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(JavaScriptSelector))]
    public class When_java_script_selector
    {
        It should_be_get_date = () => Selector.JavaScript
                                              .DateTime.GetDate()
                                              .ToString().ShouldEqual("new Date().getDate()");

        It should_be_get_day = () => Selector.JavaScript
                                             .DateTime.GetDay()
                                             .ToString().ShouldEqual("new Date().getDay()");

        It should_be_get_full_year = () => Selector.JavaScript
                                                   .DateTime.GetFullYear()
                                                   .ToString().ShouldEqual("new Date().getFullYear()");

        It should_be_get_timezone_off_set = () => Selector.JavaScript
                                                          .DateTime.GetTimezoneOffset()
                                                          .ToString().ShouldEqual("new Date().getTimezoneOffset()");

        It should_be_to_time_string = () => Selector.JavaScript
                                                    .DateTime.ToTimeString()
                                                    .ToString().ShouldEqual("new Date().toTimeString()");

        It should_be_to_date_string = () => Selector.JavaScript
                                                    .DateTime.ToDateString()
                                                    .ToString().ShouldEqual("new Date().toDateString()");
    }
}