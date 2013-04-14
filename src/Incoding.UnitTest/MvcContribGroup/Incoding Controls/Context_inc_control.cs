namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System.ComponentModel;
    using Incoding.MSpecContrib;

    #endregion

    public class Context_inc_control
    {
        #region Fake classes

        protected class FakeModel
        {
            #region Properties

            public string Prop { get; set; }

            [DisplayName("NameDisplay")]
            public string DisplayName { get; set; }

            #endregion
        }

        #endregion

        #region Static Fields

        protected static MockHtmlHelper<FakeModel> mockHtmlHelper;

        #endregion

        #region Constructors

        protected Context_inc_control()
        {
            mockHtmlHelper = MockHtmlHelper<FakeModel>
                    .When()
                    .StubModel(new FakeModel { Prop = Pleasure.Generator.TheSameString() });
        }

        #endregion
    }
}