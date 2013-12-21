namespace Incoding.UnitTest.MSpecGroup
{
    using System;
    using Incoding.CQRS;
    using Incoding.MSpecContrib;
    using Incoding.MvcContrib;
    using Machine.Specifications;

    [Subject(typeof(MockController<>))]
    public class When_mock_controller_stub_similar_query : Context_mock_controller
    {
        #region Fake classes

        class FakeSimilarQuery : QueryBase<string>
        {
            #region Properties

            public string Id { get; set; }

            #endregion

            ////ncrunch: no coverage start
            protected override string ExecuteResult()
            {
                throw new NotImplementedException();
            }

            ////ncrunch: no coverage end       
        }

        protected class FakeController : IncControllerBase
        {
            #region Api Methods

            public FakeResult FakeMethod()
            {
                return new FakeResult
                           {
                                   Result1 = this.dispatcher.Query(new FakeQuery { Id = "value" }), 
                                   Result2 = this.dispatcher.Query(new FakeSimilarQuery { Id = "value2" })
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
                                              .StubQuery(new FakeSimilarQuery { Id = "value2" }, result2);
                                  };

        Because of = () => { result = mockController.Original.FakeMethod(); };

        It should_be_strong_result = () => result.Should(fakeResult =>
                                                             {
                                                                 fakeResult.Result1.ShouldEqual(result1);
                                                                 fakeResult.Result2.ShouldEqual(result2);
                                                             });
    }
}