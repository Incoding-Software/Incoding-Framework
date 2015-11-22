namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using Incoding.Extensions;
    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(IncodingMetaCallbackTriggerDsl))]
    public class When_incoding_meta_language_dsl_trigger
    {
        #region Establish value

        static Func<JquerySelector, JquerySelectorExtend> with = selector => selector.Id("Id");

        #endregion

        It should_be_invoke = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                            .Do().Direct()
                                            .OnSuccess(dsl => dsl.With(with).Core().Trigger.Invoke(JqueryBind.DblClick))
                                            .GetExecutable<ExecutableTrigger>()
                                            .ShouldEqualData(new Dictionary<string, object>
                                                             {
                                                                     { "trigger", "dblclick" }
                                                             });

        It should_be_invoke_click = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                                  .Do().Direct()
                                                  .OnSuccess(dsl => dsl.With(with).Core().Trigger.Click())
                                                  .GetExecutable<ExecutableTrigger>()
                                                  .ShouldEqualData(new Dictionary<string, object>
                                                                   {
                                                                           { "trigger", "click" }
                                                                   });

        It should_be_invoke_for = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                                .Do().Direct()
                                                .OnSuccess(dsl => dsl.With(with).Core().Trigger.For<ArgumentException>(r => r.Message).Invoke(JqueryBind.DblClick))
                                                .GetExecutable<ExecutableTrigger>()
                                                .ShouldEqualData(new Dictionary<string, object>
                                                                 {
                                                                         { "trigger", "dblclick" },
                                                                         { "property", "Message" }
                                                                 });

        It should_be_bind_to_lower_case = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                                        .Do().Direct()
                                                        .OnSuccess(dsl => dsl.With(with).Core().Trigger.Invoke("cUsTom"))
                                                        .GetExecutable<ExecutableTrigger>()
                                                        .ShouldEqualData(new Dictionary<string, object>
                                                                         {
                                                                                 { "trigger", "custom" }
                                                                         });

        It should_be_quick_trigger = () =>
                                     {
                                         foreach (var bind in new[] { JqueryBind.Click, JqueryBind.Incoding, JqueryBind.Change, JqueryBind.None, JqueryBind.Submit, JqueryBind.Focus, JqueryBind.InitIncoding })
                                         {
                                             new IncodingMetaLanguageDsl(JqueryBind.Click)
                                                     .Do().Direct()
                                                     .OnSuccess(dsl =>
                                                                {
                                                                    var trigger = dsl.With(with).Core().Trigger;
                                                                    var getMethod = trigger.GetType().GetMethod(bind.ToString());
                                                                    getMethod.Invoke(trigger, null);
                                                                })
                                                     .GetExecutable<ExecutableTrigger>()
                                                     .ShouldEqualData(new Dictionary<string, object>
                                                                      {
                                                                              { "trigger", bind.ToJqueryString() }
                                                                      });
                                         }
                                     };
    }
}