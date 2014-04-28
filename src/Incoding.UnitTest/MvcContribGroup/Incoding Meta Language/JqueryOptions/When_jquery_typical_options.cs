namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using Incoding.MSpecContrib;
    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(MetaTypicalOptions))]
    public class When_jquery_typical_options
    {
        It should_be_set_string = () =>
                                      {
                                          var jqueryAjaxOptions = new JqueryAjaxOptions(JqueryAjaxOptions.Default);
                                          jqueryAjaxOptions.Add("Test1", Pleasure.Generator.TheSameString());
                                          jqueryAjaxOptions["Test1"].ShouldEqual(Pleasure.Generator.TheSameString());
                                      };

        It should_be_set_int = () =>
                                   {
                                       var options = new JqueryAjaxOptions(JqueryAjaxOptions.Default);
                                       options.Add("Test2", 2);
                                       options["Test2"].ShouldEqual(2);
                                   };

        It should_be_set_bool = () =>
                                    {
                                        var options = new JqueryAjaxOptions(JqueryAjaxOptions.Default);
                                        options.Add("True", true);
                                        options["True"].ShouldEqual(true);
                                    };

        It should_be_set_without_encode = () =>
                                              {
                                                  MetaTypicalOptions options = new JqueryAjaxOptions(JqueryAjaxOptions.Default);
                                                  options.Add("Test", @"ValueFor""<b>Encode</b>");
                                                  options["Test"].ShouldEqual("ValueFor\"<b>Encode</b>");
                                              };

        It should_be_default = () =>
                                   {
                                       var options = new JqueryAjaxOptions(new JqueryAjaxOptions
                                                                               {
                                                                                       Timeout = 5
                                                                               });
                                       options["timeout"].ShouldEqual(5);
                                   };
    }
}