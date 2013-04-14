namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System.Collections.Specialized;
    using Incoding.MvcContrib;
    using Machine.Specifications;using Incoding.MSpecContrib;

    #endregion

    [Subject(typeof(IncControllerBase))]
    public class When_inc_controller_base_try_push_success_with_ajax : Context_inc_controller_base
    {
        Establish establish = () => httpContext.SetupGet(r => r.Request.Headers).Returns(new NameValueCollection { { "X-Requested-With", "XMLHttpRequest" } });

        Because of = () => { result = controller.Push(new FakeCommand()); };

        It should_be_incoding_success = () => result.ShouldBeIncodingSuccess();

        It should_be_push = () => dispatcher.ShouldBePush(new FakeCommand());
    }
}