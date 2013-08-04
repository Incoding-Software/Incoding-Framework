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

    [Subject(typeof(BeginTag))]
    public class When_begin_tag
    {
        #region Estabilish value

        static Mock<TextWriter> textWriter;

        static BeginTag beginTag;

        #endregion

        Establish establish = () =>
                                  {
                                      textWriter = Pleasure.Mock<TextWriter>();
                                      var viewContext = Pleasure.MockAsObject<ViewContext>(mock => mock.SetupGet(r => r.Writer).Returns(textWriter.Object));

                                      var helper = new HtmlHelper(viewContext, Pleasure.MockAsObject<IViewDataContainer>(), new RouteCollection());
                                      beginTag = new BeginTag(helper, HtmlTag.Area, new RouteValueDictionary(new { action = "\"json\":\"value\"", method = "post" }));
                                  };

        Because of = () => beginTag.Dispose();

        It should_be_write_start = () => textWriter.Verify(r => r.Write(Pleasure.MockIt.IsStrong("<area action=\"&quot;json&quot;:&quot;value&quot;\" method=\"post\" >")));

        It should_be_write_end = () => textWriter.Verify(r => r.Write(Pleasure.MockIt.IsStrong("</area>")));
    }
}