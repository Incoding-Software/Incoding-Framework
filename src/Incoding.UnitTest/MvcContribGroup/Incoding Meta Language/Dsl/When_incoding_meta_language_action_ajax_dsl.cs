namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Web.Mvc;
    using Incoding.CQRS;
    using Incoding.Extensions;
    using Incoding.MSpecContrib;
    using Incoding.MvcContrib;
    using Incoding.MvcContrib.MVD;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(IIncodingMetaLanguageActionDsl))]
    public class When_incoding_meta_language_action_ajax_dsl
    {
        #region Fake classes

        public class FakeCommand : CommandBase
        {
            protected override void Execute()
            {
                throw new NotImplementedException();
            }
        }

        public class FakeFakeCommand : FakeCommand
        {
            protected override void Execute()
            {
                throw new NotImplementedException();
            }
        }

        public class FakeQuery : QueryBase<string>
        {
            protected override string ExecuteResult()
            {
                throw new NotImplementedException();
            }
        }

        public class FakeFakeQuery : FakeQuery
        {
            protected override string ExecuteResult()
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region Establish value

        internal static void VerifyAjax(ExecutableActionBase incodingAjaxAction, string type, bool hash, string prefix)
        {
            var ajaxOptions = incodingAjaxAction["ajax"] as Dictionary<string, object>;
            ajaxOptions.ShouldNotBeNull();
            ajaxOptions["url"].ShouldEqual(url);
            ajaxOptions["type"].ShouldEqual(type);

            incodingAjaxAction["hash"].ShouldEqual(hash);
            incodingAjaxAction["prefix"].ShouldEqual(prefix);
            incodingAjaxAction["onBind"].ShouldEqual("click incoding");

            var filterResult = incodingAjaxAction["filterResult"];
            (filterResult.TryGetValue("property") as string).ShouldEqual("Message");
            (filterResult.TryGetValue("method") as string).ShouldEqual("equal");
            (filterResult.TryGetValue("value") as string).ShouldEqual("message");
            ((bool)filterResult.TryGetValue("inverse")).ShouldEqual(false);
            (filterResult.TryGetValue("type") as string).ShouldEqual("Data");
        }

        static string url = Pleasure.Generator.Url();

        #endregion

        Establish establish = () =>
                              {
                                  var dispatcher = Pleasure.MockStrictAsObject<IUrlDispatcher>(mock =>
                                                                                               {
                                                                                                   mock.Setup(r => r.AsView("IncodingMetaLanguageDslHelper.cs")).Returns(url);
                                                                                                   mock.Setup(r => r.Query<FakeQuery>(Pleasure.MockIt.IsAny<object>())).Returns(Pleasure.MockAsObject<UrlDispatcher.IUrlQuery<FakeQuery>>(s => s.Setup(r => r.AsJson()).Returns(url)));
                                                                                                   mock.Setup(r => r.Query<FakeFakeQuery>(Pleasure.MockIt.IsAny<object>())).Returns(Pleasure.MockAsObject<UrlDispatcher.IUrlQuery<FakeFakeQuery>>(s => s.Setup(r => r.AsJson()).Returns(url)));
                                                                                                   var urlPush = Pleasure.MockAsObject<UrlDispatcher.UrlPush>(s => s.Setup(r => r.ToString()).Returns(url));
                                                                                                   mock.Setup(r => r.Push<FakeCommand>(Pleasure.MockIt.IsAny<object>())).Returns(urlPush);
                                                                                                   mock.Setup(r => r.Push<FakeFakeCommand>(Pleasure.MockIt.IsAny<object>())).Returns(urlPush);
                                                                                               });
                                  typeof(HtmlExtensions).GetField("UrlDispatcher", BindingFlags.Static | BindingFlags.NonPublic).SetValue(null, dispatcher);
                              };

        It should_be_ajax = () => new IncodingMetaLanguageDsl(JqueryBind.Click.ToString())
                                          .Do()
                                          .Ajax(url)
                                          .Where<ArgumentException>(r => r.Message == "message")
                                          .GetExecutable<ExecutableAjaxAction>()
                                          .Should(action => VerifyAjax(action, "GET", false, string.Empty));

        It should_be_ajax_get = () => new IncodingMetaLanguageDsl(JqueryBind.Click.ToString())
                                              .Do()
                                              .AjaxGet(url)
                                              .Where<ArgumentException>(r => r.Message == "message")
                                              .GetExecutable<ExecutableAjaxAction>()
                                              .Should(action => VerifyAjax(action, "GET", false, string.Empty));

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

        It should_be_ajax_mvc_html_string = () => new IncodingMetaLanguageDsl(JqueryBind.Click.ToString())
                                                          .Do()
                                                          .Ajax(new MvcHtmlString(url))
                                                          .Where<ArgumentException>(r => r.Message == "message")
                                                          .GetExecutable<ExecutableAjaxAction>()
                                                          .Should(action => VerifyAjax(action, "GET", false, string.Empty));

        It should_be_ajax_post = () => new IncodingMetaLanguageDsl("click")
                                               .Do()
                                               .AjaxPost(url)
                                               .Where<ArgumentException>(r => r.Message == "message")
                                               .GetExecutable<ExecutableAjaxAction>()
                                               .Should(action => VerifyAjax(action, "POST", false, string.Empty));

        It should_be_ajax_post_mvc_html_string = () => new IncodingMetaLanguageDsl("click")
                                                               .Do()
                                                               .AjaxPost(new MvcHtmlString(url))
                                                               .Where<ArgumentException>(r => r.Message == "message")
                                                               .GetExecutable<ExecutableAjaxAction>()
                                                               .Should(action => VerifyAjax(action, "POST", false, string.Empty));

        It should_be_ajax_dispatcher = () => new IncodingMetaLanguageDsl("click")
                                                     .Do()
                                                     .Ajax(r => r.AsView("IncodingMetaLanguageDslHelper.cs"))
                                                     .Where<ArgumentException>(r => r.Message == "message")
                                                     .GetExecutable<ExecutableAjaxAction>()
                                                     .Should(action => VerifyAjax(action, "GET", false, string.Empty));

        It should_be_ajax_query = () => new IncodingMetaLanguageDsl("click")
                                                .Do()
                                                .Ajax(new FakeQuery())
                                                .Where<ArgumentException>(r => r.Message == "message")
                                                .GetExecutable<ExecutableAjaxAction>()
                                                .Should(action => VerifyAjax(action, "GET", false, string.Empty));

        It should_be_ajax_query_performance = () => Pleasure.Do(i => new IncodingMetaLanguageDsl("click")
                                                                             .Do()
                                                                             .Ajax(new FakeQuery()), 1000)
                                                            .ShouldBeLessThan(35);

        It should_be_ajax_query_multiple_performance = () => Pleasure.Do(i => new IncodingMetaLanguageDsl("click")
                                                                                      .Do()
                                                                                      .Ajax(new FakeFakeQuery()), 1000)
                                                                     .ShouldBeLessThan(55);

        It should_be_ajax_query_multiple_level = () => new IncodingMetaLanguageDsl("click")
                                                               .Do()
                                                               .Ajax(new FakeFakeQuery())
                                                               .Where<ArgumentException>(r => r.Message == "message")
                                                               .GetExecutable<ExecutableAjaxAction>()
                                                               .Should(action => VerifyAjax(action, "GET", false, string.Empty));

        It should_be_ajax_command = () => new IncodingMetaLanguageDsl("click")
                                                  .Do()
                                                  .Ajax(new FakeCommand())
                                                  .Where<ArgumentException>(r => r.Message == "message")
                                                  .GetExecutable<ExecutableAjaxAction>()
                                                  .Should(action => VerifyAjax(action, "POST", false, string.Empty));

        It should_be_ajax_multiple_command = () => new IncodingMetaLanguageDsl("click")
                                                           .Do()
                                                           .Ajax(new FakeFakeCommand())
                                                           .Where<ArgumentException>(r => r.Message == "message")
                                                           .GetExecutable<ExecutableAjaxAction>()
                                                           .Should(action => VerifyAjax(action, "POST", false, string.Empty));

        It should_be_ajax_multiple_command_performance = () => Pleasure.Do(i => new IncodingMetaLanguageDsl("click")
                                                                                        .Do()
                                                                                        .Ajax(new FakeFakeCommand()), 1000)
                                                                       .ShouldBeLessThan(50);

        It should_be_ajax_command_performance = () => Pleasure.Do(i => new IncodingMetaLanguageDsl("click")
                                                                               .Do()
                                                                               .Ajax(new FakeCommand()), 1000)
                                                              .ShouldBeLessThan(30);

        It should_be_ajax_query_as_anonymous = () => new IncodingMetaLanguageDsl("click")
                                                             .Do()
                                                             .Ajax<FakeQuery>(new { id = "test" })
                                                             .Where<ArgumentException>(r => r.Message == "message")
                                                             .GetExecutable<ExecutableAjaxAction>()
                                                             .Should(action => VerifyAjax(action, "GET", false, string.Empty));

        It should_be_ajax_command_as_anonymous = () => new IncodingMetaLanguageDsl("click")
                                                               .Do()
                                                               .Ajax<FakeCommand>(new { id = "test" })
                                                               .Where<ArgumentException>(r => r.Message == "message")
                                                               .GetExecutable<ExecutableAjaxAction>()
                                                               .Should(action => VerifyAjax(action, "POST", false, string.Empty));

        It should_be_ajax_post_dispatcher = () => new IncodingMetaLanguageDsl("click")
                                                          .Do()
                                                          .AjaxPost(r => r.AsView("IncodingMetaLanguageDslHelper.cs"))
                                                          .Where<ArgumentException>(r => r.Message == "message")
                                                          .GetExecutable<ExecutableAjaxAction>()
                                                          .Should(action => VerifyAjax(action, "POST", false, string.Empty));
    }
}