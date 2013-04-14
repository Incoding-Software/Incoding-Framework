namespace Incoding.UnitTest.MSpecGroup
{
    #region << Using >>

    using Incoding.CQRS;
    using Incoding.MvcContrib;
    using Machine.Specifications;using Incoding.MSpecContrib;

    #endregion

    [Subject(typeof(MockController<>))]
    public class When_mock_controller_stub_query : Context_mock_controller
    {
        #region Fake classes

        protected class FakeController : IncControllerBase
        {
            #region Constructors

            public FakeController(IDispatcher dispatcher)
                    : base(dispatcher) { }

            #endregion

            #region Api Methods

            public FakeResult FakeMethod()
            {
                return new FakeResult
                           {
                                   ResultForStrongQuery = this.dispatcher.Query(new FakeQuery { Id = "value" })
                           };
            }

            #endregion
        }

        protected class FakeResult
        {
            #region Properties

            public string ResultForStrongQuery { get; set; }

            #endregion
        }

        #endregion

        #region Estabilish value

        static MockController<FakeController> mockController;

        static FakeResult result;

        static string strongResult;

        #endregion

        Establish establish = () =>
                                  {
                                      strongResult = Pleasure.Generator.String();
                                      mockController = MockController<FakeController>
                                              .When()
                                              .StubQuery(new FakeQuery { Id = "value" }, strongResult);
                                  };

        Because of = () => { result = mockController.Original.FakeMethod(); };

        It should_be_strong_result = () => result.ResultForStrongQuery.ShouldEqual(strongResult);
    }
}