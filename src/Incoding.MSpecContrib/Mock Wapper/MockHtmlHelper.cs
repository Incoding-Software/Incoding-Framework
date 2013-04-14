namespace Incoding.MSpecContrib
{
    #region << Using >>

    using System.IO;
    using System.Web.Mvc;
    using Moq;

    #endregion

    public class MockHtmlHelper<TModel>
    {
        #region Fields

        readonly Mock<TextWriter> textWriter;

        readonly Mock<ViewContext> viewContext;

        readonly Mock<IViewDataContainer> viewDataContainer;

        #endregion

        #region Constructors

        MockHtmlHelper()
        {
            this.textWriter = Pleasure.Mock<TextWriter>();

            var viewDataDictionary = new ViewDataDictionary<TModel>();

            this.viewContext = Pleasure.Mock<ViewContext>(mock =>
                                                              {
                                                                  mock.Setup(r => r.ClientValidationEnabled).Returns(true);
                                                                  mock.SetupGet(r => r.Writer).Returns(this.textWriter.Object);
                                                                  mock.SetupGet(r => r.ViewData).Returns(viewDataDictionary);
                                                              });
            this.viewDataContainer = Pleasure.Mock<IViewDataContainer>(mock => mock.SetupGet(r => r.ViewData).Returns(viewDataDictionary));
        }

        #endregion

        #region Factory constructors

        public static MockHtmlHelper<TModel> When()
        {
            return new MockHtmlHelper<TModel>();
        }

        #endregion

        #region Properties

        public HtmlHelper<TModel> Original
        {
            get { return new HtmlHelper<TModel>(this.viewContext.Object, this.viewDataContainer.Object); }
        }

        #endregion

        #region Api Methods

        public MockHtmlHelper<TModel> StubModel(TModel model)
        {
            var viewDataDictionary = new ViewDataDictionary<TModel>(model);
            this.viewDataContainer.SetupGet(r => r.ViewData).Returns(viewDataDictionary);
            this.viewContext.SetupGet(r => r.ViewData).Returns(viewDataDictionary);
            return this;
        }

        public void ShouldBeWriter(string items)
        {
            this.textWriter.Verify(writer => writer.Write(items));
        }

        public void StubFieldValidation(string prop, FieldValidationMetadata fieldValidation)
        {
            var formContext = new FormContext();
            formContext.FieldValidators.Add(prop, fieldValidation);
            this.viewContext.Setup(r => r.FormContext).Returns(formContext);
        }

        #endregion
    }
}