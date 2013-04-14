namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System.IO;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Incoding.MSpecContrib;
    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(MvcTemplate<FakeModel>))]
    public class When_mvc_template
    {
        #region Fake classes

        class FakeModel
        {
            #region Properties

            public string Price { get; set; }

            #endregion
        }

        #endregion

        #region Estabilish value

        static MvcTemplate<FakeModel> template;

        #endregion

        Establish establish = () =>
                                  {
                                      var viewContext = Pleasure.MockAsObject<ViewContext>(mock => mock.SetupGet(r => r.Writer).Returns(Pleasure.MockAsObject<TextWriter>()));
                                      template = new MvcTemplate<FakeModel>(new HtmlHelper(viewContext, Pleasure.MockAsObject<IViewDataContainer>(), new RouteCollection()));
                                  };

        It should_be_sum = () => template.Sum(r => r.Price)
                                         .ToHtmlString()
                                         .ShouldEqual("{{#IncTemplateSum}}Price{{/IncTemplateSum}}");

        It should_be_max = () => template.Max(r => r.Price)
                                         .ToHtmlString()
                                         .ShouldEqual("{{#IncTemplateMax}}Price{{/IncTemplateMax}}");

        It should_be_min = () => template.Min(r => r.Price)
                                         .ToHtmlString()
                                         .ShouldEqual("{{#IncTemplateMin}}Price{{/IncTemplateMin}}");

        It should_be_count = () => template.Count()
                                           .ToHtmlString()
                                           .ShouldEqual("{{#IncTemplateCount}}Count{{/IncTemplateCount}}");

        It should_be_first = () => template.First(r => r.Price)
                                           .ToHtmlString()
                                           .ShouldEqual("{{#IncTemplateFirst}}Price{{/IncTemplateFirst}}");

        It should_be_last = () => template.Last(r => r.Price)
                                          .ToHtmlString()
                                          .ShouldEqual("{{#IncTemplateLast}}Price{{/IncTemplateLast}}");

        It should_be_average = () => template.Average(r => r.Price)
                                             .ToHtmlString()
                                             .ShouldEqual("{{#IncTemplateAverage}}Price{{/IncTemplateAverage}}");

        It should_be_index = () => template.Index()
                                           .ToHtmlString()
                                           .ShouldEqual("{{IncTemplateIndex}}");

        It should_be_for_each = () => template.ForEach()
                                              .ShouldEqualWeak(new { property = "data" }, dsl => dsl
                                                                                                         .IncludeAllFields()
                                                                                                         .Ignore("htmlHelper", "design"));

        It should_be_not_each = () => template.NotEach()
                                              .ShouldEqualWeak(new { property = "data" }, dsl => dsl
                                                                                                         .IncludeAllFields()
                                                                                                         .Ignore("htmlHelper", "design"));

        It should_be_disposable = () => Catch
                                                .Exception(() => template.Dispose())
                                                .ShouldBeNull();
    }
}