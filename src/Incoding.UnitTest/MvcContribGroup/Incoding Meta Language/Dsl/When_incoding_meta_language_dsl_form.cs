namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System.Collections.Generic;
    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(IncodingMetaCallbackFormApiDsl))]
    public class When_incoding_meta_language_dsl_form
    {
        It should_be_validation_parse = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                                      .Do().Direct()
                                                      .OnSuccess(r => r.With(selector => selector.Id("id")).Core().Form.Validation.Parse())
                                                      .GetExecutable<ExecutableValidationParse>()
                                                      .ShouldEqualData(new Dictionary<string, object>());

        It should_be_validation_refresh = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                                        .Do().Direct()
                                                        .OnSuccess(r => r.With(selector => selector.Id("id")).Core().Form.Validation.Refresh())
                                                        .GetExecutable<ExecutableValidationRefresh>()
                                                        .ShouldEqualData(new Dictionary<string, object>());

        It should_be_reset = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                           .Do().Direct()
                                           .OnSuccess(r => r.With(selector => selector.Id("id")).Core().Form.Reset())
                                           .GetExecutable<ExecutableForm>()
                                           .ShouldEqualData(new Dictionary<string, object> { { "method", "reset" } });

        It should_be_clear = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                           .Do().Direct()
                                           .OnSuccess(r => r.With(selector => selector.Id("id")).Core().Form.Clear())
                                           .GetExecutable<ExecutableForm>()
                                           .ShouldEqualData(new Dictionary<string, object> { { "method", "clear" } });
    }
}