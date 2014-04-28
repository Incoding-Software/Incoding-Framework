namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System.Collections.Generic;
    using Incoding.MSpecContrib;
    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(IncodingMetaLanguageDsl))]
    public class When_incoding_meta_language_dsl_behaviors
    {
        #region Establish value

        static IIncodingMetaLanguageEventBuilderDsl behaviors;

        static Dictionary<string, object> data;

        #endregion

        Establish establish = () =>
                                  {
                                      data = new Dictionary<string, object>
                                                 {
                                                         { "onBind", "click incoding" }, 
                                                         { "onStatus", 2 }, 
                                                         { "target", "$('#id')" }, 
                                                         { "onEventStatus", 1 }, 
                                                 };

                                      behaviors = new IncodingMetaLanguageDsl(JqueryBind.Click)
                                              .Do()
                                              .Direct()
                                              .OnSuccess(r =>
                                                             {
                                                                 r.With(selector => selector.Id("FirstId")).Core().Trigger.Click();
                                                                 r.With(selector => selector.Id("id")).Behaviors(dsl =>
                                                                                                                     {
                                                                                                                         dsl.Core().Form.Validation.Parse();
                                                                                                                         dsl.Core().Form.Validation.Refresh();
                                                                                                                     });
                                                                 r.With(selector => selector.Id("NextId")).Core().Form.Reset();
                                                             });
                                  };

        It should_be_clear = () => behaviors.GetExecutable<ExecutableTrigger>()
                                            .ShouldEqualWeak(new Dictionary<string, object>
                                                                 {
                                                                         { "trigger", "click" },                                                                         
                                                                         { "onBind", "click incoding" },
                                                                         { "onStatus", 2 },
                                                                         { "target", "$('#FirstId')" },
                                                                         { "onEventStatus", 1 },
                                                                 });

        It should_be_parse = () => behaviors.GetExecutable<ExecutableValidationParse>()
                                            .ShouldEqualWeak(data);

        It should_be_refresh = () => behaviors.GetExecutable<ExecutableValidationRefresh>()
                                              .ShouldEqualWeak(data);

        It should_be_reset = () => behaviors.GetExecutable<ExecutableForm>()
                                            .ShouldEqualWeak(new Dictionary<string, object>
                                                                 {
                                                                         { "method", "reset" }, 
                                                                         { "onBind", "click incoding" }, 
                                                                         { "onStatus", 2 }, 
                                                                         { "target", "$('#NextId')" }, 
                                                                         { "onEventStatus", 1 }, 
                                                                 });
    }
}