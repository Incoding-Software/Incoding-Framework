namespace Incoding.UnitTest.MvcContribGroup
{
    using System.Collections.Specialized;
    using System.Web.Mvc;
    using Incoding.MSpecContrib;
    using Incoding.MvcContrib;
    using Machine.Specifications;

    [Subject(typeof(IncControllerBase))]
    public class When_inc_controller_base_try_push__multiple_error_result : Context_inc_controller_base
    {
        Establish establish = () =>
                                  {
                                      httpContext.SetupGet(r => r.Request.Headers).Returns(new NameValueCollection());
                                      dispatcher.StubPushAsThrow(new FakeCommand(), IncWebException.For("key", "message")
                                                                                                   .Also("key", "message2")
                                                                                                   .Also("another", "exception"));
                                  };

        Because of = () => { result = controller.Push(new FakeCommand()); };

        It should_be_re_view = () => result.ShouldBeModel(new FakeCommand());

        It should_be_add_model_error = () =>
                                           {
                                               var keyModelState = new ModelState();
                                               keyModelState.Errors.Add("message");
                                               keyModelState.Errors.Add("message2");
                                               controller.ModelState.ShouldBeKeyValue("key", keyModelState);

                                               var anotherModelState = new ModelState();
                                               anotherModelState.Errors.Add("exception");
                                               controller.ModelState.ShouldBeKeyValue("another", anotherModelState);
                                           };
    }
}