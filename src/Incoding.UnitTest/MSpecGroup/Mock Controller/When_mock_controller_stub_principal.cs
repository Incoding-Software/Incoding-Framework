namespace Incoding.UnitTest.MSpecGroup
{
    #region << Using >>

    using System.Security.Principal;
    using Incoding.CQRS;
    using Incoding.MSpecContrib;
    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(MockController<>))]
    public class When_mock_controller_stub_principal
    {
        #region Fake classes

        class FakeController : IncControllerBase
        {
        }

        #endregion

        #region Establish value

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