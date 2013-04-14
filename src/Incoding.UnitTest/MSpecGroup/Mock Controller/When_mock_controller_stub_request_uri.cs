namespace Incoding.UnitTest.MSpecGroup
{
    #region << Using >>

    using System;
    using Incoding.CQRS;
    using Incoding.MvcContrib;
    using Machine.Specifications;using Incoding.MSpecContrib;

    #endregion

    [Subject(typeof(MockController<>))]
    public class When_mock_controller_stub_request_uri
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

        static Uri expectedUri;

        static MockController<FakeController> mockController;

        static Uri actualUri;

        #endregion

        Establish establish = () =>
                                  {
                                      expectedUri = Pleasure.Generator.Uri();
                                      mockController = MockController<FakeController>
                                              .When()
                                              .StubRequestUrl(expectedUri);
                                  };

        Because of = () => { actualUri = mockController.Original.Request.Url; };

        It should_be_expected = () => actualUri.ShouldEqualWeak(expectedUri);
    }
}