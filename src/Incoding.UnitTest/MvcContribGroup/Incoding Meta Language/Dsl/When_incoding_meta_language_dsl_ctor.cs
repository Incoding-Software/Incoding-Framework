namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System.Linq;
    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(IncodingMetaLanguageDsl))]
    public class When_incoding_meta_language_dsl_ctor
    {
        It should_be_always_incoding = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                                    .Do()
                                                    .Direct()
                                                    .OnSuccess(r => r.Self().Core().Break.If(builder => builder.Eval("code")))
                                                    .GetAll<ExecutableDirectAction>()
                                                    .First()["onBind"]
                                                    .ShouldEqual("click incoding");

        It should_be_init_incoding = () => new IncodingMetaLanguageDsl(JqueryBind.InitIncoding)
                                                   .Do()
                                                   .Direct()
                                                   .OnSuccess(r => r.Self().Core().Break.If(builder => builder.Eval("code")))
                                                   .GetAll<ExecutableDirectAction>()
                                                   .First()["onBind"]
                                                   .ShouldEqual("initincoding incoding");

        It should_be_combine_flag = () => new IncodingMetaLanguageDsl(JqueryBind.InitIncoding | JqueryBind.IncChangeUrl)
                                                  .Do()
                                                  .Direct()
                                                  .OnSuccess(r => r.Self().Core().Break.If(builder => builder.Eval("code")))
                                                  .GetAll<ExecutableDirectAction>()
                                                  .First()["onBind"]
                                                  .ShouldEqual("initincoding incchangeurl incoding");

        It should_not_be_duplicate_incoding = () => new IncodingMetaLanguageDsl("incoding")
                                                            .Do()
                                                            .Direct()
                                                            .OnSuccess(r => r.Self().Core().Break.If(builder => builder.Eval("code")))
                                                            .GetAll<ExecutableDirectAction>()
                                                            .First()["onBind"]
                                                            .ShouldEqual("incoding");

        It should_be_always_lower = () => new IncodingMetaLanguageDsl("aBcD")
                                                  .Do()
                                                  .Direct()
                                                  .OnSuccess(r => r.Self().Core().Break.If(builder => builder.Eval("code")))
                                                  .GetAll<ExecutableDirectAction>()
                                                  .First()["onBind"]
                                                  .ShouldEqual("abcd incoding");
    }
}