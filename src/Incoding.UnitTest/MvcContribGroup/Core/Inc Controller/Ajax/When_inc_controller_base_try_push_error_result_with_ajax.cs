namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System.Collections.Generic;
    using System.Collections.Specialized;
    using Incoding.MvcContrib;
    using Machine.Specifications;using Incoding.MSpecContrib;

    #endregion

    [Subject(typeof(IncControllerBase))]
    public class When_inc_controller_base_try_push_error_result_with_ajax : Context_inc_controller_base
    {
        Establish establish = () =>
                                  {
                                      httpContext.SetupGet(r => r.Request.Headers).Returns(new NameValueCollection { { "X-Requested-With", "XMLHttpRequest" } });
                                      dispatcher.StubPushAsThrow(new FakeCommand(), IncWebException.For("key", "message"));
                                  };

        Because of = () => { result = controller.Push(new FakeCommand()); };

        It should_be_incoding_success = () => result.ShouldBeIncodingFail();

        It should_be_incoding_data = () => result.ShouldBeIncodingData<IEnumerable<object>>(o => o.ShouldEqualWeakEach(new[] { new { name = "key", isValid = false, errorMessage = "message" } }));
    }
}