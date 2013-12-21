namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System.Collections.Generic;
    using System.Web.Mvc;
    using Incoding.Extensions;
    using Incoding.MSpecContrib;
    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(JqueryAjaxFormOptions))]
    public class When_jquery_ajax_form_options
    {
        #region Estabilish value

        static JqueryAjaxFormOptions options;

        static Dictionary<string, object> originalOptions;

        #endregion

        Establish establish = () =>
                                  {
                                      originalOptions = new Dictionary<string, object>
                                                            {
                                                                    { "forceSync", Pleasure.Generator.Bool() },
                                                                    { "type", "Post" },
                                                                    { "semantic", Pleasure.Generator.Bool() },
                                                                    { "url", Pleasure.Generator.String() }
                                                            };
                                      options = new JqueryAjaxFormOptions();
                                  };

        Because of = () =>
                         {
                             foreach (var option in originalOptions)
                             {
                                 if (option.Key == "type")
                                 {
                                     options.Type = HttpVerbs.Post;
                                     continue;
                                 }
                                 string property = "{0}{1}".F(option.Key.ToCharArray()[0].ToString().ToUpper(), option.Key.Substring(1, option.Key.Length - 1));
                                 options.SetValue(property, option.Value);
                             }
                         };

        It should_be_compare = () => options.ShouldEqualWeakEach(originalOptions);
    }
}