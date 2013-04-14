namespace Incoding.UnitTest.MSpecGroup
{
    #region << Using >>

    using System.Security.Principal;
    using Incoding.CQRS;
    using Incoding.MvcContrib;
    using Machine.Specifications;using Incoding.MSpecContrib;

    #endregion

    [Subject(typeof(MockController<>))]
    public class When_mock_controller_stub_principal
    {
        #region Fake classes

        class FakeController : IncControllerBase
        {
            #region Constructors

            public FakeController(IDispatcher dispatcher)
                    : base(dispatcher) { }

            #endregion
        }

        #endregion

        #region Estabilish value

        static MockController<FakeController> mockController;

        static IPrincipal principal;

        #endregion

        Establish establish = () =>
                                  {
                                      mockController = MockController<FakeController>
                                              .When()
                                              .StubPrincipal(Pleasure.MockAsObject<IPrincipal>());
                                  };

        Because of = () => { principal = mockController.Original.User; };

        It should_be_principal = () => principal.ShouldNotBeNull();
    }
}