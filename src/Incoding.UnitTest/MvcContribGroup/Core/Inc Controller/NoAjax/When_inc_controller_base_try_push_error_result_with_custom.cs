namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System.Collections.Specialized;
    using System.Web.Mvc;
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
                             result = controller.PushErrorResult(new FakeCommand(), exception => new ContentResult
                                                                                                     {
                                                                                                             Content = exception.Message
                                                                                                     });
                         };

        It should_be_re_view = () => ((ContentResult)result).Should(contentResult => contentResult.Content.ShouldEqual(Pleasure.Generator.TheSameString()));
    }
}