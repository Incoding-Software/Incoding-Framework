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
    public class When_mock_controller_with_inc_view : Context_mock_controller
    {
        #region Fake classes

        protected class FakeController : IncControllerBase
        {

            #region Api Methods

            public IncodingResult FakeMethod()
            {
                // ReSharper disable Mvc.ViewNotResolved
                return IncView(10);

                // ReSharper restore Mvc.ViewNotResolved
            }

            #endregion
        }

        #endregion

        #region Estabilish value

        static MockController<FakeController> mockController;

        static IncodingResult result;

        #endregion

        Establish establish = () =>
                                  {
                                      mockController = MockController<FakeController>
                                              .When();
                                  };

        Because of = () => { result = mockController.Original.FakeMethod(); };

        It should_be_result = () => result.ShouldBeIncodingSuccess();

        It should_be_render = () => mockController.ShouldBeRenderModel(10);

        It should_be_render_by_action = () => mockController.ShouldBeRenderModel<int>(i => i.ShouldEqual(10));

        It should_not_be_render_self_without_model = () => Catch.Exception(() => mockController.ShouldBeRenderView()).ShouldBeOfType<MockException>();
    }
}