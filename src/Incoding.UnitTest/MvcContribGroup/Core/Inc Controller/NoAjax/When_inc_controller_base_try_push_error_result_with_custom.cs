namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System.Collections.Specialized;
    using System.Linq;
    using System.Web.Mvc;
    using Incoding.Extensions;
    using Incoding.MSpecContrib;
    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(IncControllerBase))]
    public class When_inc_controller_base_try_push_error_result_with_custom : Context_inc_controller_base
    {
        Establish establish = () =>
                                  {
                                      httpContext.SetupGet(r => r.Request.Headers).Returns(new NameValueCollection());
                                      dispatcher.StubPushAsThrow(new FakeCommand(), IncWebException.For("key", Pleasure.Generator.TheSameString()));
                                  };

        Because of = () =>
                         {
                             result = controller.PushErrorResult(new FakeCommand(), exception =>
                                                                                        {
                                                                                            var pair = exception.Errors.First();
                                                                                            return new ContentResult
                                                                                                       {
                                                                                                               Content = "{0}:{1}".F(pair.Key, pair.Value[0])
                                                                                                       };
                                                                                        });
                         };

        It should_be_re_view = () => ((ContentResult)result).Should(contentResult => contentResult.Content.ShouldEqual("key:TheSameString"));
    }
}