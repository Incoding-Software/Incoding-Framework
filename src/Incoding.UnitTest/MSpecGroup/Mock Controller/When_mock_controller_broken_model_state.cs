namespace Incoding.UnitTest.MSpecGroup
{
    #region << Using >>

    using Incoding.CQRS;
    using Incoding.MSpecContrib;
    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(MockController<>))]
    public class When_mock_controller_broken_model_state
    {
        #region Fake classes

        class FakeController : IncControllerBase
        {
        }

        #endregion

        #region Establish value

        static MockController<FakeController> mockController;

        static bool isValid;

        #endregion

        Establish establish = () =>
                                  {
                                      mockController = MockController<FakeController>
                                              .When()
                                              .BrokenModelState();
                                  };

        Because of = () => { isValid = mockController.Original.ModelState.IsValid; };

        It should_be_broken = () => isValid.ShouldBeFalse();
    }
}