namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System.IO;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Incoding.Block.IoC;
    using Incoding.MSpecContrib;
    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(MvcTemplate<FakeModel>))]
    public class When_mvc_template
    {
        #region Fake classes

        class FakeModel { }

        #endregion

        #region Establish value

        static MvcTemplate<FakeModel> template;

        #endregion

        #region Establish value

        static HtmlHelper htmlHelper;

        #endregion

        Establish establish = () =>
                                  {                                      
                                      var viewContext = Pleasure.MockAsObject<ViewContext>(mock => mock.SetupGet(r => r.Writer).Returns(Pleasure.MockAsObject<TextWriter>()));
                                      htmlHelper = new HtmlHelper(viewContext, Pleasure.MockAsObject<IViewDataContainer>(), new RouteCollection());
                                      var syntax = new TemplateHandlebarsSyntax<FakeModel>(htmlHelper, string.Empty, HandlebarsType.Each, string.Empty);
                                      var factory = Pleasure.MockAsObject<ITemplateFactory>(mock =>
                                                                                                {
                                                                                                    mock.Setup(r => r.ForEach<FakeModel>(Pleasure.MockIt.IsNotNull<HtmlHelper>()))
                                                                                                        .Returns(syntax);
                                                                                                    mock.Setup(r => r.NotEach<FakeModel>(Pleasure.MockIt.IsNotNull<HtmlHelper>()))
                                                                                                        .Returns(syntax);
                                                                                                });
                                      IoCFactory.Instance.StubTryResolve(factory);
                                  };

        It should_be_for_each = () => new MvcTemplate<FakeModel>(htmlHelper).ForEach()
                                                                            .ShouldNotBeNull();

        It should_be_not_each = () => new MvcTemplate<FakeModel>(htmlHelper).NotEach()
                                                                            .ShouldNotBeNull();

        It should_be_disposable = () => Catch
                                                .Exception(() => new MvcTemplate<FakeModel>(htmlHelper).Dispose())
                                                .ShouldBeNull();
    }
}