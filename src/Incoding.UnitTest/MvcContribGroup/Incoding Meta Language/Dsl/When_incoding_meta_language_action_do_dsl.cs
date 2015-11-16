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
                                                          .DoWithPreventDefault()
                                                          .Direct()
                                                          .GetExecutable<ExecutableDirectAction>()
                                                          ["onEventStatus"].ShouldEqual(2);

        It should_be_without_event_behavior = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                                            .Direct()
                                                            .GetExecutable<ExecutableDirectAction>()
                                                            ["onEventStatus"].ShouldEqual(1);

        It should_be_prevent_default = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                                     .PreventDefault()
                                                     .Direct()
                                                     .GetExecutable<ExecutableDirectAction>()
                                                     ["onEventStatus"].ShouldEqual(2);

        It should_be_prevent_default_without_direct = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                                                    .PreventDefault()
                                                                    .ShouldNotBeNull();
        
        It should_be_stop_propagation_without_direct = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                                                    .StopPropagation()
                                                                    .ShouldNotBeNull();
                                                     

        It should_be_with_stop_propagation = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                                           .DoWithStopPropagation().Direct()
                                                           .GetExecutable<ExecutableDirectAction>()
                                                           ["onEventStatus"].ShouldEqual(3);

        It should_be_stop_propagation = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                                      .StopPropagation().Direct()
                                                      .GetExecutable<ExecutableDirectAction>()
                                                      ["onEventStatus"].ShouldEqual(3);

        It should_be_with_prevent_default_and_stop_propagation = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                                                               .DoWithPreventDefaultAndStopPropagation()
                                                                               .Direct()
                                                                               .GetExecutable<ExecutableDirectAction>()
                                                                               ["onEventStatus"].ShouldEqual(4);

        It should_be_with_prevent_default_and_stop_propagation_new = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                                                                   .PreventDefault()
                                                                                   .StopPropagation()
                                                                                   .Direct()
                                                                                   .GetExecutable<ExecutableDirectAction>()
                                                                                   ["onEventStatus"].ShouldEqual(4);

        It should_be_prevent_default_and_stop_propagation = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                                                          .PreventDefault()
                                                                          .StopPropagation()
                                                                          .Direct()
                                                                          .GetExecutable<ExecutableDirectAction>()
                                                                          ["onEventStatus"].ShouldEqual(4);

        It should_be_multiple_prevent_default_then_none = () =>
                                                          {
                                                              var meta = new IncodingMetaLanguageDsl(JqueryBind.Click)
                                                                      .PreventDefault()
                                                                      .Direct()
                                                                      .OnSuccess(dsl => { })
                                                                      .When(JqueryBind.Change)
                                                                      .Submit();

                                                              meta.GetExecutable<ExecutableDirectAction>()["onEventStatus"].ShouldEqual(2);
                                                              meta.GetExecutable<ExecutableSubmitAction>()["onEventStatus"].ShouldEqual(1);
                                                          };

        It should_be_multiple_none_then_prevent_default = () =>
                                                          {
                                                              var meta = new IncodingMetaLanguageDsl(JqueryBind.Click)
                                                                      .Direct()
                                                                      .OnSuccess(dsl => { })
                                                                      .When(JqueryBind.Change)
                                                                      .PreventDefault()
                                                                      .Submit();

                                                              meta.GetExecutable<ExecutableDirectAction>()["onEventStatus"].ShouldEqual(1);
                                                              meta.GetExecutable<ExecutableSubmitAction>()["onEventStatus"].ShouldEqual(2);
                                                          };
    }
}