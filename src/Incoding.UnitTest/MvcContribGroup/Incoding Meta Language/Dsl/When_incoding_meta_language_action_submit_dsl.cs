namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System.Collections.Generic;
    using Incoding.MSpecContrib;
    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(IIncodingMetaLanguageActionDsl))]
    public class When_incoding_meta_language_action_submit_dsl
    {
        It should_be_submit = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                            .Do().Submit()
                                            .GetExecutable<ExecutableSubmitAction>()
                                            .Should(action =>
                                                        {
                                                            action.Data["formSelector"].ShouldEqual(Selector.Jquery.Self().ToString());
                                                            action.Data["options"].ShouldBeOfType<Dictionary<string, object>>();
                                                        });

        It should_be_submit_on = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                               .Do().SubmitOn(selector => selector.Id("id").Parent())
                                               .GetExecutable<ExecutableSubmitAction>()
                                               .Should(action =>
                                                           {
                                                               action.Data["formSelector"].ShouldEqual("$('#id').parent()");
                                                               action.Data["options"].ShouldBeOfType<Dictionary<string, object>>();
                                                           });
    }
}