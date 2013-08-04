namespace Incoding.UnitTest.MSpecGroup
{
    #region << Using >>

    using Incoding.CQRS;
    using Incoding.MSpecContrib;
    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(MockController<>))]
    public class When_mock_controller_stub_query_string
    {
        #region Fake classes

        class FakeController : IncControllerBase
        {
            #region Constructors

            public FakeController(IDispatcher dispatcher)
                    : base(dispatcher) { }

            #endregion

            #region Api Methods

            public string GetQueryString(string key)
            {
                return HttpContext.Request.QueryString[key];
            }

            #endregion
        }

        #endregion

        #region Estabilish value

        static MockController<FakeController> mockController;

        static string valFromQueryString;

        #endregion

        Establish establish = () =>
                                  {
                                      mockController = MockController<FakeController>
                                              .When()
                                              .StubQueryString(new { aws = Pleasure.Generator.TheSameString() });
                                  };

        Because of = () => { valFromQueryString = mockController.Original.GetQueryString("aws"); };

        It should_be_value = () => valFromQueryString.ShouldEqual(Pleasure.Generator.TheSameString());
    }
}