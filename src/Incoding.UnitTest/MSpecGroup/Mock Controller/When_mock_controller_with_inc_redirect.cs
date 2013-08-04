namespace Incoding.UnitTest.MSpecGroup
{
    #region << Using >>

    using Incoding.CQRS;
    using Incoding.Extensions;
    using Incoding.MSpecContrib;
    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(MockController<>))]
    public class When_mock_controller_with_inc_redirect : Context_mock_controller
    {
        #region Fake classes

        protected class FakeController : IncControllerBase
        {
            #region Constructors

            public FakeController(IDispatcher dispatcher)
                    : base(dispatcher) { }

            #endregion

            #region Api Methods

            public IncodingResult RedirectTo(string action, string controller, object routes)
            {
                return IncRedirectToAction(action, controller, routes);
            }

            public IncodingResult RedirectTo(string action, string controller)
            {
                return IncRedirectToAction(action, controller);
            }

            public IncodingResult RedirectTo(string action)
            {
                return IncRedirectToAction(action);
            }

            public IncodingResult RedirectToDirect(string url)
            {
                return IncRedirect(url);
            }

            #endregion
        }

        #endregion

        #region Estabilish value

        static MockController<FakeController> mockController;

        #endregion

        Establish establish = () =>
                                  {
                                      mockController = MockController<FakeController>
                                              .When()
                                              .StubUrlAction("/Controller/Action")
                                              .StubUrlAction("/IncController/Action")
                                              .StubUrlAction("/IncController/Action".AppendToQueryString(new { param = Pleasure.Generator.TheSameString() }));
                                  };

        It should_be_redirect_to = () => mockController.Original.RedirectTo("Action").ShouldBeIncodingRedirect("/Controller/Action");

        It should_be_redirect_to_controller = () => mockController.Original.RedirectTo("Action", "IncController").ShouldBeIncodingRedirect("/IncController/Action");

        It should_be_redirect_to_routes = () => mockController.Original.RedirectTo("Action", "IncController", new { param = Pleasure.Generator.TheSameString() }).ShouldBeIncodingRedirect("/IncController/Action?param=TheSameString");

        It should_be_redirect_to_url = () => mockController.Original.RedirectToDirect(Pleasure.Generator.TheSameString()).ShouldBeIncodingRedirect(Pleasure.Generator.TheSameString());
    }
}