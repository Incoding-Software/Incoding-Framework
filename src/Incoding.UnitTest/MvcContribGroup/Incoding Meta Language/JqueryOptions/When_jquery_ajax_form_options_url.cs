namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System.Collections.Generic;
    using Incoding.MSpecContrib;
    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(JqueryAjaxFormOptions))]
    public class When_jquery_ajax_form_options_url
    {
        #region Establish value

        static JqueryAjaxFormOptions options;

        #endregion

        Establish establish = () => { options = new JqueryAjaxFormOptions(JqueryAjaxFormOptions.Default); };

        Because of = () => options.Url = "http://controller/action?param1=#id1&param2=#id2";

        It should_be_has_data = () =>
                                    {
                                        var dataAsCollection = options["data"] as IEnumerable<JqueryAjaxRoute>;
                                        dataAsCollection.ShouldEqualWeakEach(Pleasure.ToList(new JqueryAjaxRoute { name = "param1", value = "#id1" },
                                                                                             new JqueryAjaxRoute { name = "param2", value = "#id2" }));
                                    };

        It should_be_clear_url = () => options["url"].ShouldEqual("http://controller/action");
    }
}