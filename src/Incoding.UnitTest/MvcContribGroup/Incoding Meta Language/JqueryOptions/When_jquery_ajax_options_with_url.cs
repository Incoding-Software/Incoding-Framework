namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System.Collections.Generic;
    using Incoding.MSpecContrib;
    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(JqueryAjaxOptions))]
    public class When_jquery_ajax_options_with_url
    {
        #region Establish value

        static JqueryAjaxOptions options;

        #endregion

        Establish establish = () => { options = new JqueryAjaxOptions(JqueryAjaxOptions.Default); };

        Because of = () => options.Url = "/Dispatcher/Query?type=IsAuthorizeUserQuery&UserId=||cookie*OwnerId||&CustomerId=$('#id')";

        It should_be_has_data = () =>
                                    {
                                        var dataAsCollection = options["data"] as IEnumerable<object>;
                                        dataAsCollection.ShouldEqualWeakEach(Pleasure.ToEnumerable<object>(new { name = "type", selector = "IsAuthorizeUserQuery" },
                                                                                                           new { name = "UserId", selector = "||cookie*OwnerId||" },
                                                                                                           new { name = "CustomerId", selector = "$('#id')" }));
                                    };

        It should_be_clear_url = () => options["url"].ShouldEqual("/Dispatcher/Query");
    }
}