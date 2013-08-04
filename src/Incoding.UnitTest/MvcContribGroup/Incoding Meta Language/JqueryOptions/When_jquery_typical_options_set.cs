namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using Incoding.MSpecContrib;
    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(MetaTypicalOptions))]
    public class When_jquery_typical_options_set
    {
        It should_be_set_string = () =>
                                      {
                                          var jqueryAjaxOptions = new JqueryAjaxOptions(JqueryAjaxOptions.Default);
                                          jqueryAjaxOptions.Set("Test1", Pleasure.Generator.TheSameString());
                                          jqueryAjaxOptions.OptionCollections["Test1"].ShouldEqual(Pleasure.Generator.TheSameString());
                                      };

        It should_be_set_int = () =>
                                   {
                                       var options = new JqueryAjaxOptions(JqueryAjaxOptions.Default);
                                       options.Set("Test2", 2);
                                       options.OptionCollections["Test2"].ShouldEqual(2);
                                   };

        It should_be_set_bool = () =>
                                    {
                                        var options = new JqueryAjaxOptions(JqueryAjaxOptions.Default);
                                        options.Set("True", true);
                                        options.OptionCollections["True"].ShouldEqual(true);
                                    };

        It should_be_replace_old = () =>
                                       {
                                           var options = new JqueryAjaxOptions(JqueryAjaxOptions.Default);
                                           options.Set("Test", true);
                                           options.Set("Test", Pleasure.Generator.TheSameString());

                                           options.OptionCollections["Test"].ShouldEqual(Pleasure.Generator.TheSameString());
                                       };

        It should_be_set_without_encode = () =>
                                              {
                                                  MetaTypicalOptions options = new JqueryAjaxOptions(JqueryAjaxOptions.Default);
                                                  options.Set("Test", @"ValueFor""<b>Encode</b>");
                                                  options.OptionCollections["Test"].ShouldEqual("ValueFor\"<b>Encode</b>");
                                              };
    }
}