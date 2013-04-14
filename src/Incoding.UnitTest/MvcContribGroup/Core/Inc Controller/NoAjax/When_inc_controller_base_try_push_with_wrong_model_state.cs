namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System.Collections.Specialized;
    using System.Web.Mvc;
    using Incoding.MvcContrib;
    using Machine.Specifications;using Incoding.MSpecContrib;

    #endregion

    [Subject(typeof(IncControllerBase))]
    public class When_inc_controller_base_try_push_with_wrong_model_state : Context_inc_controller_base
    {
        Establish establish = () =>
                                  {
                                      httpContext.SetupGet(r => r.Request.Headers).Returns(new NameValueCollection());
                                      modelStateDictionary.AddModelError("key", "message");
                                  };

        Because of = () => { result = controller.Push(new FakeCommand()); };

        It should_be_re_view = () => result.ShouldBeModel(new FakeCommand());

        It should_be_add_model_error = () =>
                                           {
                                               var modelState = new ModelState();
                                               modelState.Errors.Add("message");
                                               controller.ModelState.ShouldBeKeyValue("key", modelState);
                                           };
    }
}