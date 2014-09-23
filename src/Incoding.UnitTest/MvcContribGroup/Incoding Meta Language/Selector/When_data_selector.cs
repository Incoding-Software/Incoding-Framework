namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(ResultSelector))]
    public class When_data_selector
    {
        #region Fake classes

        class FakeModel
        {
            #region Properties

            public string Test { get; set; }

            #endregion
        }

        #endregion

        It should_be_data = () => Selector.Result
                                          .ToString()
                                          .ShouldEqual("||result||");

        It should_be_data_for = () => Selector.Result.For<FakeModel>(r => r.Test)
                                              .ToString()
                                              .ShouldEqual("||result*Test||");
    }
}