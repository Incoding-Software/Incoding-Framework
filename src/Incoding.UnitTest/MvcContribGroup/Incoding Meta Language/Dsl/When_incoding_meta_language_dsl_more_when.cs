namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System.Linq;
    using Incoding.MvcContrib;
    using Machine.Specifications;using Incoding.MSpecContrib;

    #endregion

    [Subject(typeof(IncodingMetaLanguageDsl))]
    public class When_incoding_meta_language_dsl_more_when
    {
        #region Estabilish value

        static IIncodingMetaLanguageEventBuilderDsl metaBuilder;

        #endregion

        Because of = () =>
                         {
                             metaBuilder = new IncodingMetaLanguageDsl(JqueryBind.Click)
                                     .Do().Direct()
                                     .OnSuccess(r => r.Self().Core().Eval(Pleasure.Generator.TheSameString()))
                                     .When(JqueryBind.Blur)
                                     .Do().Direct()
                                     .OnSuccess(r => r.Self().Core().Eval(Pleasure.Generator.TheSameString()))
                                     .When("load")
                                     .Do().Direct()
                                     .OnSuccess(r => r.Self().Core().Eval(Pleasure.Generator.TheSameString()));
                         };

        It should_be_action_when_click = () => metaBuilder
                                                       .GetActions<ExecutableDirectAction>()
                                                       .SingleOrDefault(r => r.Data["onBind"].ToString() == "click incoding")
                                                       .ShouldNotBeNull();

        It should_be_callback_when_click = () => metaBuilder
                                                         .GetActions<ExecutableEval>()
                                                         .SingleOrDefault(r => r.Data["onBind"].ToString() == "click incoding")
                                                         .ShouldNotBeNull();

        It should_be_action_when_blur = () => metaBuilder
                                                      .GetActions<ExecutableDirectAction>()
                                                      .SingleOrDefault(r => r.Data["onBind"].ToString() == "blur incoding")
                                                      .ShouldNotBeNull();

        It should_be_callback_when_blur = () => metaBuilder
                                                        .GetActions<ExecutableEval>()
                                                        .SingleOrDefault(r => r.Data["onBind"].ToString() == "blur incoding")
                                                        .ShouldNotBeNull();

        It should_be_action_when_load = () => metaBuilder
                                                      .GetActions<ExecutableDirectAction>()
                                                      .SingleOrDefault(r => r.Data["onBind"].ToString() == "load incoding")
                                                      .ShouldNotBeNull();

        It should_be_callback_when_load = () => metaBuilder
                                                        .GetActions<ExecutableEval>()
                                                        .SingleOrDefault(r => r.Data["onBind"].ToString() == "load incoding")
                                                        .ShouldNotBeNull();
    }
}