namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System.Linq;
    using System.Web.Routing;
    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(JqueryAjaxOptions))]
    public class When_jquery_ajax_options_with_data
    {
        #region Establish value

        static JqueryAjaxOptions options;

        static RouteValueDictionary data = new RouteValueDictionary(new { value1 = 1, value2 = 2 });

        #endregion

        It should_be_get = () =>
                           {
                               options = new JqueryAjaxOptions(new JqueryAjaxOptions());
                               options.Data = data;
                               options.Data.ShouldEqual(data);
                           };

        It should_be_set_by_default = () =>
                                      {
                                          var def = JqueryAjaxOptions.Default;
                                          def.Data = new RouteValueDictionary(new { Id = "id" });
                                          options = new JqueryAjaxOptions(def);
                                          options.Data.ShouldEqual(def.Data);
                                      };

        It should_be_set_data_and_then_url = () =>
                                             {
                                                 options = new JqueryAjaxOptions(new JqueryAjaxOptions());
                                                 options.Data = data;
                                                 options.Url = "/Test/Home";

                                                 new[] { options["url"], options.Url }.All(o => o.ToString() == "/Test/Home?value1=1&value2=2").ShouldBeTrue();
                                             };

        It should_be_set_data_and_then_url_scenario_rap_appointment = () =>
                                                                      {
                                                                          options = new JqueryAjaxOptions(new JqueryAjaxOptions());
                                                                          options.Data = new RouteValueDictionary(new
                                                                                                                  {
                                                                                                                          inc_csrf_token = Selector.JS.Eval("AjaxAdapter.Token"),
                                                                                                                          CurrentAdmissionId = Selector.Incoding.QueryString("CurrentAdmissionId"),
                                                                                                                          CurrentOrderId = Selector.Incoding.QueryString("CurrentOrderId"),
                                                                                                                  });
                                                                          options.Url = "/Dispatcher/Query?incType=GetDetailForAppointmentNameQuery&Id=$(this.self)";
                                                                          new[] { options["url"], options.Url }.All(o => o.ToString() == "/Dispatcher/Query?incType=GetDetailForAppointmentNameQuery&Id=$(this.self)&inc_csrf_token=||javascript*AjaxAdapter.Token||&CurrentAdmissionId=||queryString*CurrentAdmissionId||&CurrentOrderId=||queryString*CurrentOrderId||").ShouldBeTrue();
                                                                      };

        It should_be_set_data_and_then_url_with_jquery_old = () =>
                                                             {
                                                                 options = new JqueryAjaxOptions(new JqueryAjaxOptions());
                                                                 options.Data = data;
                                                                 options.Url = "/Test/Home?value3=$('#aws')";
                                                                 
                                                                 new[] { options["url"], options.Url }.All(o => o.ToString() == "/Test/Home?value3=$('%23aws')&value1=1&value2=2").ShouldBeTrue();
                                                             };

        It should_be_set_data_with_empty_url = () =>
                                               {
                                                   options = new JqueryAjaxOptions(new JqueryAjaxOptions());
                                                   options.Data = data;

                                                   new[] { options["url"], options.Url }.All(o => o.ToString() == "?value1=1&value2=2").ShouldBeTrue();
                                               };

        It should_be_set_url_and_then_data = () =>
                                             {
                                                 options = new JqueryAjaxOptions(new JqueryAjaxOptions());
                                                 options.Url = "/Test/Home";
                                                 options.Data = data;

                                                 new[] { options["url"], options.Url }.All(o => o.ToString() == "/Test/Home?value1=1&value2=2").ShouldBeTrue();
                                             };

        It should_be_set_data_with_null_url = () =>
                                              {
                                                  options = new JqueryAjaxOptions(new JqueryAjaxOptions());
                                                  options.Data = data;

                                                  new[] { options["url"], options.Url }.All(o => o.ToString() == "?value1=1&value2=2").ShouldBeTrue();
                                              };
    }
}