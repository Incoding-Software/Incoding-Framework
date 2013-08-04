namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System.IO;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Incoding.MSpecContrib;
    using Incoding.MvcContrib;
    using Machine.Specifications;
    using Moq;
    using It = Machine.Specifications.It;

    #endregion

    [Subject(typeof(MvcScriptTemplate<>))]
    public class When_mvc_script_template
    {
        #region Estabilish value

        static Mock<TextWriter> textWriter;

        static MvcScriptTemplate<string> script;

        #endregion

        Establish establish = () =>
                                  {
                                      textWriter = Pleasure.Mock<TextWriter>();
                                      var viewContext = Pleasure.MockAsObject<ViewContext>(mock => mock.SetupGet(r => r.Writer).Returns(textWriter.Object));

                                      var helper = new HtmlHelper(viewContext, Pleasure.MockAsObject<IViewDataContainer>(), new RouteCollection());
                                      script = new MvcScriptTemplate<string>(helper, "id", "htmlType");
                                  };

        Because of = () => script.Dispose();

        It should_be_write_start = () => textWriter.Verify(r => r.Write(Pleasure.MockIt.IsStrong("<script id=\"id\" type=\"htmlType\">")));

        It should_be_write_end = () => textWriter.Verify(r => r.Write(Pleasure.MockIt.IsStrong("</script>")));
    }
}