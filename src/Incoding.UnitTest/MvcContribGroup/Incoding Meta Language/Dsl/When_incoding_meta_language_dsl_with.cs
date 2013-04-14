namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System.Linq;
    using Incoding.MvcContrib;
    using Machine.Specifications;using Incoding.MSpecContrib;

    #endregion

    [Subject(typeof(IncodingMetaLanguageDsl))]
    public class When_incoding_meta_language_dsl_with
    {
        It should_be_self = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                          .Do()
                                          .Direct()
                                          .OnSuccess(r => r.Self().Core().Form.Validation.Parse())
                                          .GetActions<ExecutableValidationParse>()
                                          .First()
                                          .Data["target"]
                                          .ShouldEqual("$(this.self)");

        It should_be_with = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                          .Do()
                                          .Direct()
                                          .OnSuccess(dsl => dsl.With(Selector.Jquery.Self().Closest(s => s.Tag(HtmlTag.Tr))).Core().Form.Validation.Parse())
                                          .GetActions<ExecutableValidationParse>()
                                          .First()
                                          .Data["target"]
                                          .ShouldEqual("$(this.self).closest('tr')");

        It should_be_with_action = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                                 .Do()
                                                 .Direct()
                                                 .OnSuccess(dsl => dsl.With(r => r.Self().Closest(s => s.Tag(HtmlTag.Tr))).Core().Form.Validation.Parse())
                                                 .GetActions<ExecutableValidationParse>()
                                                 .First()
                                                 .Data["target"]
                                                 .ShouldEqual("$(this.self).closest('tr')");
    }
}