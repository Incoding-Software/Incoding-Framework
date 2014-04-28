namespace Incoding.UnitTest.MSpecGroup
{
    #region << Using >>

    using Incoding.CQRS;
    using Incoding.MSpecContrib;
    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(MockController<>))]
    public class When_mock_controller_stub_url_action
    {
        #region Fake classes

        protected class FakeController : IncControllerBase
        {

            #region Api Methods

            public string FakeMethod()
            {
                // ReSharper disable Mvc.ControllerNotResolved
                // ReSharper disable Mvc.ActionNotResolved
                return Url.Action("Test", "Home");

                // ReSharper restore Mvc.ActionNotResolved
                // ReSharper restore Mvc.ControllerNotResolved
            }

            public string FakeMethod2()
            {
                // ReSharper disable Mvc.ControllerNotResolved
                // ReSharper disable Mvc.ActionNotResolved
                return Url.Action("aws", "Home");

                // ReSharper restore Mvc.ActionNotResolved
                // ReSharper restore Mvc.ControllerNotResolved
            }

            #endregion
        }

        #endregion

        #region Establish value

        static MockController<FakeController> mockController;

        #endregion

        Establish establish = () =>
                                  {
                                      mockController = MockController<FakeController>
                                              .When()
                                              .StubUrlAction("/Home/Test")
                                              .StubUrlAction(s => s.ShouldContain("aws"), "aws");
                                  };

        It should_be_stub_url = () => mockController.Original.FakeMethod().ShouldEqual("/Home/Test");

        It should_be_stub_action_url = () => mockController.Original.FakeMethod2().ShouldEqual("aws");
    }
}