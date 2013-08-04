namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System.Collections.Generic;
    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(IncodingMetaLanguageDsl))]
    public class When_incoding_meta_language_dsl_behaviors
    {
        #region Estabilish value

        static IIncodingMetaLanguageEventBuilderDsl behaviors;

        #endregion

        Establish establish = () =>
                                  {
                                      behaviors = new IncodingMetaLanguageDsl(JqueryBind.Click)
                                              .Do().Direct()
                                              .OnSuccess(r => r.With(selector => selector.Id("id")).Behaviors(dsl =>
                                                                                                                  {
                                                                                                                      dsl.Core().Form.Validation.Parse();
                                                                                                                      dsl.Core().Form.Validation.Refresh();
                                                                                                                  }));
                                  };

        It should_be_parse = () => behaviors.GetExecutable<ExecutableValidationParse>()
                                            .ShouldEqualData(new Dictionary<string, object>());

        It should_be_refresh = () => behaviors.GetExecutable<ExecutableValidationRefresh>()
                                              .ShouldEqualData(new Dictionary<string, object>());
    }
}