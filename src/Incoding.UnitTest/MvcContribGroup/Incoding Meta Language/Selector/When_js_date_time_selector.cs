namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(JSDateTimeSelector))]
    public class When_js_date_time_selector
    {
        #region Estabilish value

        static JSDateTimeSelector date;

        #endregion

        Because of = () => { date = Selector.JS.DateTime; };

        It should_be_get_date = () => date.GetDate()
                                          .ToString()
                                          .ShouldEqual("@@javascript:new Date().getDate()@@");

        It should_be_get_day = () => date.GetDay()
                                         .ToString().ShouldEqual("@@javascript:new Date().getDay()@@");

        It should_be_get_full_year = () => date.GetFullYear()
                                               .ToString().ShouldEqual("@@javascript:new Date().getFullYear()@@");

        It should_be_get_timezone_off_set = () => date.GetTimezoneOffset()
                                                      .ToString().ShouldEqual("@@javascript:new Date().getTimezoneOffset()@@");

        It should_be_to_time_string = () => date.ToTimeString()
                                                .ToString().ShouldEqual("@@javascript:new Date().toTimeString()@@");

        It should_be_to_date_string = () => date.ToDateString()
                                                .ToString().ShouldEqual("@@javascript:new Date().toDateString()@@");
    }
}