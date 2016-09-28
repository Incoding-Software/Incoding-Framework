namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using Incoding.MSpecContrib;
    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(IIncodingMetaLanguageActionDsl))]
    public class When_incoding_meta_language_action_direct_dsl
    {
        It should_be_direct = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                            .Do().Direct()
                                            .GetExecutable<ExecutableDirectAction>()
                                            .Should(action => action["result"].ShouldNotBeNull());

        It should_be_direct_with_data = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                                      .Do()
                                                      .Direct(Pleasure.Generator.TheSameString())
                                                      .GetExecutable<ExecutableDirectAction>()
                                                      .Should(action => action["result"].ShouldEqual("{\"success\":true,\"data\":\"TheSameString\",\"redirectTo\":\"\",\"statusCode\":200}"));

        It should_be_direct_with_incoding_result = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                                                 .Do().Direct(IncodingResult.Success(Pleasure.Generator.TheSameString()))
                                                                 .GetExecutable<ExecutableDirectAction>()
                                                                 .Should(action => action["result"].ShouldEqual("{\"success\":true,\"data\":\"TheSameString\",\"redirectTo\":\"\",\"statusCode\":200}"));
    }
}