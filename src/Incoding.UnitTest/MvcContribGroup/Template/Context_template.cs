namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System.Collections.Generic;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    public class Context_template
    {
        #region Fake classes

        protected class FakeModel
        {
            #region Properties

            public string Name { get; set; }

            public bool Is { get; set; }

            public List<FakeModel> Items { get; set; }

            public FakeModel Fake { get; set; }

            #endregion
        }

        #endregion

        #region Static Fields

        protected static MockHtmlHelper<object> htmlHelper;

        #endregion

        #region Fields

        Establish establish = () =>
                                  {
                                      htmlHelper = MockHtmlHelper<object>
                                              .When();
                                  };

        #endregion
    }

}