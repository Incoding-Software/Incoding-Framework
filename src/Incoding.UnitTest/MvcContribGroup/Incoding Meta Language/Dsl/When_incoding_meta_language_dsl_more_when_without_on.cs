namespace Incoding.UnitTest.MvcContribGroup
{
    using System.Linq;
    using Incoding.MSpecContrib;
    using Incoding.MvcContrib;
    using Machine.Specifications;

    [Subject(typeof(IncodingMetaLanguageDsl))]
    public class When_incoding_meta_language_dsl_more_when_without_on
    {
        #region Establish value

        static IIncodingMetaLanguageEventBuilderDsl metaBuilder;

        #endregion

        Because of = () =>
                     {
                         metaBuilder = new IncodingMetaLanguageDsl(JqueryBind.Click)
                                 .Do().Direct()
                                 .When(JqueryBind.Blur)
                                 .Do().Direct()
                                 .OnSuccess(r => r.Self().Core().Eval(Pleasure.Generator.TheSameString()))
                                 .When("load")
                                 .Do().Direct()
                                 .OnSuccess(r => r.Self().Core().Eval(Pleasure.Generator.TheSameString()));
                     };

        It should_be_action_when_blur = () => metaBuilder
                                                      .GetActions<ExecutableDirectAction>()
                                                      .SingleOrDefault(r => r["onBind"].ToString() == "blur incoding")
                                                      .ShouldNotBeNull();

        It should_be_action_when_click = () => metaBuilder
                                                       .GetActions<ExecutableDirectAction>()
                                                       .SingleOrDefault(r => r["onBind"].ToString() == "click incoding")
                                                       .ShouldNotBeNull();

        It should_be_action_when_load = () => metaBuilder
                                                      .GetActions<ExecutableDirectAction>()
                                                      .SingleOrDefault(r => r["onBind"].ToString() == "load incoding")
                                                      .ShouldNotBeNull();

        It should_be_callback_when_blur = () => metaBuilder
                                                        .GetActions<ExecutableEval>()
                                                        .SingleOrDefault(r => r["onBind"].ToString() == "blur incoding")
                                                        .ShouldNotBeNull();

        It should_be_callback_when_click = () => metaBuilder
                                                         .GetActions<ExecutableEval>()
                                                         .SingleOrDefault(r => r["onBind"].ToString() == "click incoding")
                                                         .ShouldNotBeNull();

        It should_be_callback_when_load = () => metaBuilder
                                                        .GetActions<ExecutableEval>()
                                                        .SingleOrDefault(r => r["onBind"].ToString() == "load incoding")
                                                        .ShouldNotBeNull();
    }
}