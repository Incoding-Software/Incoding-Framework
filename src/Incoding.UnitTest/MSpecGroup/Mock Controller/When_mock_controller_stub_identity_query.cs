namespace Incoding.UnitTest.MSpecGroup
{
    #region << Using >>

    using Incoding.MSpecContrib;
    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(MockController<>))]
    public class When_mock_controller_stub_identity_query : Context_mock_controller
    {
        #region Fake classes

        protected class FakeController : IncControllerBase
        {
            #region Api Methods

            public FakeResult FakeMethod()
            {
                return new FakeResult
                           {
                                   Result1 = dispatcher.Query(new FakeQuery { Id = "value" }), 
                                   Result2 = dispatcher.Query(new FakeQuery { Id = "value2" })
                           };
            }

            #endregion
        }

        protected class FakeResult
        {
            #region Properties

            public string Result1 { get; set; }

            public string Result2 { get; set; }

            #endregion
        }

        #endregion

        #region Establish value

        static MockController<FakeController> mockController;

        static FakeResult result;

        static string result1;

        static string result2;

        #endregion

        Establish establish = () =>
                                  {
                                      result1 = Pleasure.Generator.String();
                                      result2 = Pleasure.Generator.String();
                                      mockController = MockController<FakeController>
                                              .When()
                                              .StubQuery(new FakeQuery { Id = "value" }, result1)
                                              .StubQuery(new FakeQuery { Id = "value2" }, result2);
                                  };

        Because of = () => { result = mockController.Original.FakeMethod(); };

        It should_be_strong_result = () => result.Should(fakeResult =>
                                                             {
                                                                 fakeResult.Result1.ShouldEqual(result1);
                                                                 fakeResult.Result2.ShouldEqual(result2);
                                                             });
    }
}