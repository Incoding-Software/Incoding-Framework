namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System.Collections.Generic;
    using Incoding.Extensions;
    using Incoding.MSpecContrib;
    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(JqueryAjaxOptions))]
    public class When_jquery_ajax_options
    {
        #region Estabilish value

        static JqueryAjaxOptions options;

        static Dictionary<string, object> originalOptions;

        #endregion

        Establish establish = () =>
                                  {
                                      originalOptions = new Dictionary<string, object>
                                                            {
                                                                    { "cache", Pleasure.Generator.Bool() }, 
                                                                    { "crossDomain", Pleasure.Generator.Bool() }, 
                                                                    { "global", Pleasure.Generator.Bool() }, 
                                                                    { "async", Pleasure.Generator.Bool() }, 
                                                                    { "traditional", Pleasure.Generator.Bool() }, 
                                                                    { "timeout", Pleasure.Generator.PositiveNumber() }, 
                                                            };
                                      options = new JqueryAjaxOptions();
                                  };

        Because of = () =>
                         {
                             foreach (var option in originalOptions)
                             {
                                 string property = "{0}{1}".F(option.Key.ToCharArray()[0].ToString().ToUpper(), option.Key.Substring(1, option.Key.Length - 1));
                                 options.SetValue(property, option.Value);
                             }
                         };

        It should_be_compare = () => options.OptionCollections.ShouldEqualWeak(originalOptions);
    }
}