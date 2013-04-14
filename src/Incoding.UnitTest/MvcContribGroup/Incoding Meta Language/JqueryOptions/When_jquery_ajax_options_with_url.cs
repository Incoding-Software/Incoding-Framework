namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using Incoding.MvcContrib;
    using Machine.Specifications;using Incoding.MSpecContrib;

    #endregion

    [Subject(typeof(JqueryAjaxOptions))]
    public class When_jquery_ajax_options_with_url
    {
        #region Estabilish value

        static JqueryAjaxOptions options;

        #endregion

        Establish establish = () => { options = new JqueryAjaxOptions(JqueryAjaxOptions.Default); };

        Because of = () => options.WithUrl("http://controller/action?param1=#id1&param2=#id2");

        It should_be_has_data = () => options.OptionCollections["data"].ShouldEqual("[{\"name\":\"param1\",\"selector\":\"#id1\"},{\"name\":\"param2\",\"selector\":\"#id2\"}]");

        It should_be_clear_url = () => options.OptionCollections["url"].ShouldEqual("http://controller/action");
    }
}