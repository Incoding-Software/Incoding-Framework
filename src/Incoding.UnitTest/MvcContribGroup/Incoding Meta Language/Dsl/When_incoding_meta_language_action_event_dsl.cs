namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using Incoding.MvcContrib;
    using Machine.Specifications;using Incoding.MSpecContrib;

    #endregion

    [Subject(typeof(IIncodingMetaLanguageActionDsl))]
    public class When_incoding_meta_language_action_event_dsl
    {
        It should_be_event = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                           .Do().Event()
                                           .GetExecutable<ExecutableEventAction>()
                                           .ShouldNotBeNull();
    }
}