namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System.Collections.Generic;
    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(IncodingMetaCallbackBindDsl))]
    public class When_incoding_meta_language_dsl_bind
    {
        It should_be_attach = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                            .OnSuccess(dsl => dsl.Self()
                                                                 .Core()
                                                                 .Bind
                                                                 .Attach(languageDsl => languageDsl
                                                                                                .When(JqueryBind.Click)
                                                                                                .Do()
                                                                                                .Direct()
                                                                                                .OnSuccess(r => r.Utilities.Window.Alert("message"))))
                                            .GetExecutable<ExecutableBind>()
                                            .ShouldEqualData(new Dictionary<string, object>
                                                                 {
                                                                         { "type", "attach" }, 
                                                                         { "meta", @"[{""type"":""ExecutableDirectAction"",""data"":{""result"":"""",""onBind"":""click incoding"",""onStatus"":0,""target"":null,""onEventStatus"":1}},{""type"":""ExecutableEvalMethod"",""data"":{""method"":""alert"",""args"":[""message""],""context"":""window"",""onBind"":""click incoding"",""onStatus"":2,""target"":null,""onEventStatus"":1}}]" }
                                                                 });

        It should_be_detach = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                            .OnSuccess(dsl => dsl.Self().Core().Bind.Detach(JqueryBind.Click))
                                            .GetExecutable<ExecutableBind>()
                                            .ShouldEqualData(new Dictionary<string, object>
                                                                 {
                                                                         { "type", "detach" }, 
                                                                         { "bind", "click" }
                                                                 });

        It should_be_detach_all = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                                .OnSuccess(dsl => dsl.Self().Core().Bind.DetachAll())
                                                .GetExecutable<ExecutableBind>()
                                                .ShouldEqualData(new Dictionary<string, object>
                                                                     {
                                                                             { "type", "detach" }
                                                                     });
    }
}