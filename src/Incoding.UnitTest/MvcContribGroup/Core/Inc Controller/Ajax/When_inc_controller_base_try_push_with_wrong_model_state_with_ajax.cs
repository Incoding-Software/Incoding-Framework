namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System.Collections.Generic;
    using System.Collections.Specialized;
    using Incoding.MSpecContrib;
    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(IncControllerBase))]
    public class When_inc_controller_base_try_push_with_wrong_model_state_with_ajax : Context_inc_controller_base
    {
        Establish establish = () =>
                                  {
                                      httpContext.SetupGet(r => r.Request.Headers).Returns(new NameValueCollection { { "X-Requested-With", "XMLHttpRequest" } });
                                      modelStateDictionary.AddModelError("key", "message");
                                  };

        Because of = () => { result = controller.Push(new FakeCommand()); };

        It should_be_incoding_success = () => result.ShouldBeIncodingError();

        It should_be_incoding_data = () => result.ShouldBeIncodingData<IEnumerable<object>>(o => o.ShouldEqualWeakEach(new[] { new { name = "key", isValid = false, errorMessage = "message" } }));

        It should_be_push = () => dispatcher.ShouldBePush(new FakeCommand(), callCount: 0);
    }
}