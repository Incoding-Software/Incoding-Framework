namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System.Linq;
    using System.Web.Routing;
    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(JqueryAjaxFormOptions))]
    public class When_jquery_ajax_form_options_data
    {
        #region Establish value

        static JqueryAjaxFormOptions options;

        static RouteValueDictionary data = new RouteValueDictionary(new { value1 = 1, value2 = 2 });

        #endregion

        It should_be_get = () =>
                           {
                               options = new JqueryAjaxFormOptions(new JqueryAjaxFormOptions());
                               options.Data = data;
                               options.Data.ShouldEqual(data);
                           };

        It should_be_set_data_and_then_url = () =>
                                             {
                                                 options = new JqueryAjaxFormOptions(new JqueryAjaxFormOptions());
                                                 options.Data = data;
                                                 options.Url = "/Test/Home";

                                                 new[] { options["url"], options.Url }.All(o => o.ToString() == "/Test/Home?value1=1&value2=2").ShouldBeTrue();
                                             };

        It should_be_ctor = () =>
                            {
                                var def = new JqueryAjaxFormOptions();
                                def.Data = data;
                                options = new JqueryAjaxFormOptions(def);

                                new[] { options["url"], options.Url }.All(o => o.ToString() == "?value1=1&value2=2").ShouldBeTrue();
                            };

        It should_be_set_data_with_empty_url = () =>
                                               {
                                                   options = new JqueryAjaxFormOptions(new JqueryAjaxFormOptions());
                                                   options.Url = string.Empty;
                                                   options.Data = data;

                                                   new[] { options["url"], options.Url }.All(o => o.ToString() == "?value1=1&value2=2").ShouldBeTrue();
                                               };

        It should_be_set_data_with_null_url = () =>
                                              {
                                                  options = new JqueryAjaxFormOptions(new JqueryAjaxFormOptions());
                                                  options.Data = data;

                                                  new[] { options["url"], options.Url }.All(o => o.ToString() == "?value1=1&value2=2").ShouldBeTrue();
                                              };

        It should_be_set_url_and_then_data = () =>
                                             {
                                                 options = new JqueryAjaxFormOptions(new JqueryAjaxFormOptions());
                                                 options.Url = "/Test/Home";
                                                 options.Data = data;

                                                 new[] { options["url"], options.Url }.All(o => o.ToString() == "/Test/Home?value1=1&value2=2").ShouldBeTrue();
                                             };
    }
}