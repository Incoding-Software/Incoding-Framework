namespace Incoding.UnitTest.MSpecGroup
{
    #region << Using >>

    using Incoding.CQRS;
    using Incoding.MSpecContrib;
    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(MockController<>))]
    public class When_mock_controller_with_inc_json : Context_mock_controller
    {
        #region Fake classes

        protected class FakeController : IncControllerBase
        {

            #region Api Methods

            public IncodingResult FakeMethod()
            {
                // ReSharper disable Mvc.ViewNotResolved
                return IncJson(10);

                // ReSharper restore Mvc.ViewNotResolved
            }

            #endregion
        }

        #endregion

        #region Establish value

        static MockController<FakeController> mockController;

        static IncodingResult result;

        #endregion

        Establish establish = () =>
                                  {
                                      mockController = MockController<FakeController>
                                              .When();
                                  };

        Because of = () => { result = mockController.Original.FakeMethod(); };

        It should_be_result = () => result.ShouldBeIncodingData(10);
    }
}