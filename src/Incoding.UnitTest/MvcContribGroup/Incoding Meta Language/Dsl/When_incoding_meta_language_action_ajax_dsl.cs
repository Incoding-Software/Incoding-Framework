namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using Incoding.Extensions;
    using Incoding.MSpecContrib;
    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(IIncodingMetaLanguageActionDsl))]
    public class When_incoding_meta_language_action_ajax_dsl
    {
        #region Estabilish value

        internal static void VerifyAjax(ExecutableActionBase incodingAjaxAction, string type, bool hash, string prefix)
        {
            var ajaxOptions = incodingAjaxAction.Data["ajax"] as Dictionary<string, object>;
            ajaxOptions.ShouldNotBeNull();
            ajaxOptions["url"].ShouldEqual(url);
            ajaxOptions["type"].ShouldEqual(type);

            incodingAjaxAction.Data["hash"].ShouldEqual(hash);
            incodingAjaxAction.Data["prefix"].ShouldEqual(prefix);
            incodingAjaxAction.Data["onBind"].ShouldEqual("click incoding");

            var filterResult = incodingAjaxAction.Data["filterResult"];
            (filterResult.TryGetValue("property") as string).ShouldEqual("Message");
            (filterResult.TryGetValue("method") as string).ShouldEqual("equal");
            (filterResult.TryGetValue("value") as string).ShouldEqual("message");
            ((bool)filterResult.TryGetValue("inverse")).ShouldEqual(false);
            (filterResult.TryGetValue("type") as string).ShouldEqual("Data");
        }

        static string url = Pleasure.Generator.Url();

        #endregion

        It should_be_ajax_get = () => new IncodingMetaLanguageDsl(JqueryBind.Click.ToString())
                                              .Do()
                                              .AjaxGet(url)
                                              .Where<ArgumentException>(r => r.Message == "message")
                                              .GetExecutable<ExecutableAjaxAction>()
                                              .Should(action => VerifyAjax(action, "GET", false, string.Empty));

        It should_be_ajax_post = () => new IncodingMetaLanguageDsl("click")
                                               .Do()
                                               .AjaxPost(url)
                                               .Where<ArgumentException>(r => r.Message == "message")
                                               .GetExecutable<ExecutableAjaxAction>()
                                               .Should(action => VerifyAjax(action, "POST", false, string.Empty));

        It should_be_ajax_hash_get = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                                   .Do()
                                                   .AjaxHashGet(url)
                                                   .Where<ArgumentException>(r => r.Message == "message")
                                                   .GetExecutable<ExecutableAjaxAction>()
                                                   .Should(action => VerifyAjax(action, "GET", true, "root"));

        It should_be_ajax_hash_post = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                                    .Do()
                                                    .AjaxHashPost(url)
                                                    .Where<ArgumentException>(r => r.Message == "message")
                                                    .GetExecutable<ExecutableAjaxAction>()
                                                    .Should(action => VerifyAjax(action, "POST", true, "root"));
    }
}