namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System.Collections.Generic;
    using Incoding.Block.Logging;
    using Incoding.MSpecContrib;
    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(ExecutableBreak))]
    public class When_incoding_meta_language_dsl_utilities
    {
        It should_be_break = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                           .Do().Direct()
                                           .OnSuccess(r => r.Self().Core().Break.If(builder => builder.Eval("code")))
                                           .GetExecutable<ExecutableBreak>()
                                           .ShouldNotBeNull();

        It should_be_break_from_root = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                                     .Do().Direct()
                                                     .OnSuccess(r => r.Break.If(builder => builder.Eval("code")))
                                                     .GetExecutable<ExecutableBreak>()
                                                     .ShouldNotBeNull();

        It should_be_call = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                          .Do().Direct()
                                          .OnSuccess(r => r.Self().Call("Func", "id"))
                                          .GetExecutable<ExecutableEvalMethod>()
                                          .ShouldEqualData(new Dictionary<string, object>
                                                           {
                                                                   { "method", "Func" },
                                                                   { "args", new[] { "id" } },
                                                                   { "context", string.Empty },
                                                           });

        It should_be_document_back = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                                   .Do().Direct()
                                                   .OnSuccess(r => r.Utilities.Document.Back())
                                                   .GetExecutable<ExecutableEvalMethod>()
                                                   .ShouldEqualData(new Dictionary<string, object>
                                                                    {
                                                                            { "method", "go" },
                                                                            { "args", new[] { "-1" } },
                                                                            { "context", "history" },
                                                                    });

        It should_be_document_forward = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                                      .Do().Direct()
                                                      .OnSuccess(r => r.Utilities.Document.Forward())
                                                      .GetExecutable<ExecutableEvalMethod>()
                                                      .ShouldEqualData(new Dictionary<string, object>
                                                                       {
                                                                               { "method", "go" },
                                                                               { "args", new[] { "1" } },
                                                                               { "context", "history" },
                                                                       });

        It should_be_document_history_go = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                                         .Do().Direct()
                                                         .OnSuccess(r => r.Utilities.Document.HistoryGo(5))
                                                         .GetExecutable<ExecutableEvalMethod>()
                                                         .ShouldEqualData(new Dictionary<string, object>
                                                                          {
                                                                                  { "method", "go" },
                                                                                  { "args", new[] { "5" } },
                                                                                  { "context", "history" },
                                                                          });

        It should_be_document_set_title = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                                        .Do().Direct()
                                                        .OnSuccess(r => r.Utilities.Document.Title("Title"))
                                                        .GetExecutable<ExecutableEval>()
                                                        .ShouldEqualData(new Dictionary<string, object>
                                                                         {
                                                                                 { "code", "document.title = 'Title';" }
                                                                         });

        It should_be_eval = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                          .Do().Direct()
                                          .OnSuccess(r => r.Self().Core().Eval(Pleasure.Generator.TheSameString()))
                                          .GetExecutable<ExecutableEval>()
                                          .ShouldEqualData(new Dictionary<string, object>
                                                           {
                                                                   { "code", Pleasure.Generator.TheSameString() }
                                                           });

        It should_be_redirect_to = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                                 .Do().Direct()
                                                 .OnSuccess(r => r.Utilities.Document.RedirectTo(Pleasure.Generator.TheSameString()))
                                                 .GetExecutable<ExecutableEvalMethod>()
                                                 .ShouldEqualData(new Dictionary<string, object>
                                                                  {
                                                                          { "method", "RedirectTo" },
                                                                          { "args", new[] { "TheSameString" } },
                                                                          { "context", "ExecutableHelper" },
                                                                  });

        It should_be_redirect_to_self = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                                      .Do().Direct()
                                                      .OnSuccess(r => r.Utilities.Document.RedirectToSelf())
                                                      .GetExecutable<ExecutableEvalMethod>()
                                                      .ShouldEqualData(new Dictionary<string, object>
                                                                       {
                                                                               { "method", "RedirectTo" },
                                                                               { "args", new[] { "||javascript*window.location.href||" } },
                                                                               { "context", "ExecutableHelper" },
                                                                       });

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

        It should_be_window_alert = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                                  .Do().Direct()
                                                  .OnSuccess(r => r.Utilities.Window.Alert("Title"))
                                                  .GetExecutable<ExecutableEvalMethod>()
                                                  .ShouldEqualData(new Dictionary<string, object>
                                                                   {
                                                                           { "method", "alert" },
                                                                           { "args", new[] { "Title" } },
                                                                           { "context", "window" },
                                                                   });

        It should_be_window_alert_encode = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                                         .Do().Direct()
                                                         .OnSuccess(r => r.Utilities.Window.Alert("Title '"))
                                                         .GetExecutable<ExecutableEvalMethod>()
                                                         .ShouldEqualData(new Dictionary<string, object>
                                                                          {
                                                                                  { "method", "alert" },
                                                                                  { "args", new[] { "Title '" } },
                                                                                  { "context", "window" },
                                                                          });

        It should_be_window_alert_with_selector = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                                                .Do().Direct()
                                                                .OnSuccess(r => r.Utilities.Window.Alert(Selector.Jquery.Id("id")))
                                                                .GetExecutable<ExecutableEvalMethod>()
                                                                .ShouldEqualData(new Dictionary<string, object>
                                                                                 {
                                                                                         { "method", "alert" },
                                                                                         { "args", new[] { "$('#id')" } },
                                                                                         { "context", "window" },
                                                                                 });

        It should_be_window_clear_interval = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                                           .Do().Direct()
                                                           .OnSuccess(r => r.Utilities.Window.ClearInterval(Pleasure.Generator.TheSameString()))
                                                           .GetExecutable<ExecutableEval>()
                                                           .ShouldEqualData(new Dictionary<string, object>
                                                                            {
                                                                                    { "code", "window.clearInterval(ExecutableBase.IntervalIds['TheSameString'])" }
                                                                            });

        It should_be_window_console_log = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                                        .Do().Direct()
                                                        .OnSuccess(r => r.Utilities.Window.Console.Log(LogType.Fatal, Selector.Jquery.Class("test")))
                                                        .GetExecutable<ExecutableEvalMethod>()
                                                        .ShouldEqualData(new Dictionary<string, object>
                                                                         {
                                                                                 { "method", "log" },
                                                                                 { "args", new[] { "Fatal", "$('.test')" } },
                                                                                 { "context", "window.console" },
                                                                         });

        It should_be_window_open = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                                 .Do().Direct()
                                                 .OnSuccess(r => r.Utilities.Window.Open(Pleasure.Generator.TheSameString(), "Title"))
                                                 .GetExecutable<ExecutableEvalMethod>()
                                                 .ShouldEqualData(new Dictionary<string, object>
                                                                  {
                                                                          { "method", "open" },
                                                                          { "args", new[] { "TheSameString", "Title" } },
                                                                          { "context", "window" },
                                                                  });
    }
}