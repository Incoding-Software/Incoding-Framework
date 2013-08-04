namespace Incoding.UnitTest.MSpecGroup
{
    #region << Using >>

    using Incoding.CQRS;
    using Incoding.MSpecContrib;
    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(MockController<>))]
    public class When_mock_controller_with_inc_view_without_model : Context_mock_controller
    {
        #region Fake classes

        protected class FakeController : IncControllerBase
        {
            #region Constructors

            public FakeController(IDispatcher dispatcher)
                    : base(dispatcher) { }

            #endregion

            #region Api Methods

            public IncodingResult FakeMethod()
            {
                // ReSharper disable Mvc.ViewNotResolved
                return IncView();

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

        It should_be_render_null = () => mockController.ShouldBeRenderModel(null);

        It should_be_render_without_model = () => mockController.ShouldBeRenderView();
    }
}