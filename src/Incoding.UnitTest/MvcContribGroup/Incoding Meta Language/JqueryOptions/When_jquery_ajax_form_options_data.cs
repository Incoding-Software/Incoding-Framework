namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System.Collections.Generic;
    using Incoding.MSpecContrib;
    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(JqueryAjaxFormOptions))]
    public class When_jquery_ajax_form_options_data
    {
        #region Establish value

        static JqueryAjaxFormOptions options;

        #endregion

        #region Establish value

        static IEnumerable<JqueryAjaxRoute> data;

        #endregion

        Establish establish = () =>
                                  {
                                      options = new JqueryAjaxFormOptions(JqueryAjaxFormOptions.Default);
                                      data = Pleasure.ToList(new JqueryAjaxRoute { name = "param1", value = "#id1" },
                                                             new JqueryAjaxRoute { name = "param2", value = "#id2" });
                                  };

        Because of = () => options.Data = data;

        It should_be_data = () =>
                                {
                                    var dataAsCollection = options["data"] as IEnumerable<JqueryAjaxRoute>;
                                    dataAsCollection.ShouldEqualWeakEach(data);
                                };
    }
}