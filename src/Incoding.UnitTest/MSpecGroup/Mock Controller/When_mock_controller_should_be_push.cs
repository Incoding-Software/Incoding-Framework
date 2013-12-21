namespace Incoding.UnitTest.MSpecGroup
{
    #region << Using >>

    using Incoding.CQRS;
    using Incoding.MSpecContrib;
    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(MockController<>))]
    public class When_mock_controller_should_be_push : Context_mock_controller
    {
        #region Fake classes

        protected class FakeController : IncControllerBase
        {

            #region Api Methods

            public void FakeMethod()
            {
                this.dispatcher.Push(new FakeCommand { FakeProp = Pleasure.Generator.TheSameString() });
            }

            #endregion
        }

        #endregion

        #region Estabilish value

        static MockController<FakeController> mockController;

        static string strongResult;

        #endregion

        Establish establish = () =>
                                  {
                                      strongResult = Pleasure.Generator.String();
                                      mockController = MockController<FakeController>
                                              .When();
                                  };

        Because of = () => mockController.Original.FakeMethod();

        It should_be_push = () => mockController.ShouldBePush(new FakeCommand { FakeProp = Pleasure.Generator.TheSameString() });

        It should_not_be_push = () => mockController.ShouldNotBePush(Pleasure.Generator.Invent<FakeCommand>());

        It should_be_push_by_action = () => mockController.ShouldBePush<FakeCommand>(command => command.ShouldNotBeNull());
    }

}