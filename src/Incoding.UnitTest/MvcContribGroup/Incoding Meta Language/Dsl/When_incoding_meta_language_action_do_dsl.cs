namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(IIncodingMetaLanguageActionDsl))]
    public class When_incoding_meta_language_action_do_dsl
    {
        It should_be_with_prevent_default = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                                          .DoWithPreventDefault().Direct()
                                                          .GetExecutable<ExecutableDirectAction>()
                                                          ["onEventStatus"].ShouldEqual(2);

        It should_be_with_stop_propagation = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                                           .DoWithStopPropagation().Direct()
                                                           .GetExecutable<ExecutableDirectAction>()
                                                           ["onEventStatus"].ShouldEqual(3);

        It should_be_with_prevent_default_and_stop_propagation = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                                                               .DoWithPreventDefaultAndStopPropagation().Direct()
                                                                               .GetExecutable<ExecutableDirectAction>()
                                                                               ["onEventStatus"].ShouldEqual(4);
    }
}