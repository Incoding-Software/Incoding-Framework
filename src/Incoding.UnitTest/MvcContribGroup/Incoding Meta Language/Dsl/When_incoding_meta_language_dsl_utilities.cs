namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System.Collections.Generic;
    using Incoding.MvcContrib;
    using Machine.Specifications;using Incoding.MSpecContrib;

    #endregion

    [Subject(typeof(ExecutableBreak))]
    public class When_incoding_meta_language_dsl_utilities
    {
        It should_be_break = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                           .Do().Direct()
                                           .OnSuccess(r => r.Self().Core().Break.If(builder => builder.Eval("code")))
                                           .GetExecutable<ExecutableBreak>()
                                           .ShouldNotBeNull();

        It should_be_redirect_to = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                                 .Do().Direct()
                                                 .OnSuccess(r => r.Utilities.Document.RedirectTo(Pleasure.Generator.TheSameString()))
                                                 .GetExecutable<ExecutableRedirect>()
                                                 .ShouldEqualData(new Dictionary<string, object>
                                                                      {
                                                                              { "redirectTo", Pleasure.Generator.TheSameString() }
                                                                      });

        It should_be_redirect_to_self = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                                      .Do().Direct()
                                                      .OnSuccess(r => r.Utilities.Document.RedirectToSelf())
                                                      .GetExecutable<ExecutableRedirect>()
                                                      .ShouldEqualData(new Dictionary<string, object>());

        It should_be_reload = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                            .Do().Direct()
                                            .OnSuccess(r => r.Utilities.Document.Reload())
                                            .GetExecutable<ExecutableEval>()
                                            .ShouldEqualData(new Dictionary<string, object>
                                                                 {
                                                                         { "code", "window.location.reload(false);" }
                                                                 });

        It should_be_reload_with_force = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                                       .Do().Direct()
                                                       .OnSuccess(r => r.Utilities.Document.Reload(true))
                                                       .GetExecutable<ExecutableEval>()
                                                       .ShouldEqualData(new Dictionary<string, object>
                                                                            {
                                                                                    { "code", "window.location.reload(true);" }
                                                                            });

        It should_be_eval = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                          .Do().Direct()
                                          .OnSuccess(r => r.Self().Core().Eval(Pleasure.Generator.TheSameString()))
                                          .GetExecutable<ExecutableEval>()
                                          .ShouldEqualData(new Dictionary<string, object>
                                                               {
                                                                       { "code", Pleasure.Generator.TheSameString() }
                                                               });

        It should_be_document_history_go = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                                         .Do().Direct()
                                                         .OnSuccess(r => r.Utilities.Document.HistoryGo(5))
                                                         .GetExecutable<ExecutableEval>()
                                                         .ShouldEqualData(new Dictionary<string, object>
                                                                              {
                                                                                      { "code", "history.go(5);" }
                                                                              });

        It should_be_document_back = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                                   .Do().Direct()
                                                   .OnSuccess(r => r.Utilities.Document.Back())
                                                   .GetExecutable<ExecutableEval>()
                                                   .ShouldEqualData(new Dictionary<string, object>
                                                                        {
                                                                                { "code", "history.go(-1);" }
                                                                        });

        It should_be_document_forward = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                                      .Do().Direct()
                                                      .OnSuccess(r => r.Utilities.Document.Forward())
                                                      .GetExecutable<ExecutableEval>()
                                                      .ShouldEqualData(new Dictionary<string, object>
                                                                           {
                                                                                   { "code", "history.go(1);" }
                                                                           });

        It should_be_document_set_title = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                                        .Do().Direct()
                                                        .OnSuccess(r => r.Utilities.Document.Title("Title"))
                                                        .GetExecutable<ExecutableEval>()
                                                        .ShouldEqualData(new Dictionary<string, object>
                                                                             {
                                                                                     { "code", "document.title = 'Title';" }
                                                                             });

        It should_be_window_alert = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                                  .Do().Direct()
                                                  .OnSuccess(r => r.Utilities.Window.Alert("Title"))
                                                  .GetExecutable<ExecutableEval>()
                                                  .ShouldEqualData(new Dictionary<string, object>
                                                                       {
                                                                               { "code", "window.alert('Title');" }
                                                                       });

        It should_be_window_clear_interval = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                                           .Do().Direct()
                                                           .OnSuccess(r => r.Utilities.Window.ClearInterval(Pleasure.Generator.TheSameString()))
                                                           .GetExecutable<ExecutableEval>()
                                                           .ShouldEqualData(new Dictionary<string, object>
                                                                                {
                                                                                        { "code", "window.clearInterval(ExecutableBase.IntervalIds['TheSameString'])" }
                                                                                });
    }
}