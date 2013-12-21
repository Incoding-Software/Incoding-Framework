namespace Incoding.UnitTest.MSpecGroup
{
    #region << Using >>

    using Incoding.CQRS;
    using Incoding.MSpecContrib;
    using Incoding.MvcContrib;
    using Machine.Specifications;
    using Moq;
    using It = Machine.Specifications.It;

    #endregion

    [Subject(typeof(MockController<>))]
    public class When_mock_controller_with_ctor : Context_mock_controller
    {
        #region Fake classes

        protected class FakeController : IncControllerBase
        {
            #region Fields

            readonly IFakeInterface fakeInterface;

            #endregion

            #region Constructors

            public FakeController(IFakeInterface fakeInterface)
            {
                this.fakeInterface = fakeInterface;
            }

            #endregion

            #region Api Methods

            public void FakeMethod()
            {
                dispatcher.Push(new FakeCommand());
                this.fakeInterface.FakeMethod();
            }

            #endregion
        }

        #endregion

        #region Estabilish value

        static MockController<FakeController> mockController;

        static Mock<IFakeInterface> fakeInterface;

        #endregion

        Establish establish = () =>
                                  {
                                      fakeInterface = Pleasure.Mock<IFakeInterface>();
                                      mockController = MockController<FakeController>
                                              .When(fakeInterface.Object);
                                  };

        Because of = () => mockController.Original.FakeMethod();

        It should_be_push_null = () => mockController.ShouldBePush(new FakeCommand());

        It should_be_fake_interface = () => fakeInterface.Verify(r => r.FakeMethod());
    }
}