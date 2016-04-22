namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Incoding.Block.IoC;
    using Incoding.CQRS;
    using Incoding.MSpecContrib;
    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(TemplateHandlebarsOnServerSide))]
    public class When_template_handlebars
    {
        private static RenderViewQuery query;

        private static HtmlHelper htmlHelper;

        private static object data;

        Establish establish = () =>
                              {
                                  data = new { data = new List<KeyValueVm>() { new KeyValueVm(1), new KeyValueVm(2) } };
                                  htmlHelper = new HtmlHelper(new ViewContext(), Pleasure.MockStrictAsObject<IViewDataContainer>(), new RouteCollection());
                                  var dispatcher = Pleasure.Mock<IDispatcher>();
                                  query = Pleasure.Generator.Invent<RenderViewQuery>(dsl => dsl.Tuning(r => r.HtmlHelper, htmlHelper));
                                  var tmpl = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "handlebars_sample_tmpl.txt"));
                                  dispatcher.StubQuery(query, new MvcHtmlString(tmpl));
                                  IoCFactory.Instance.StubTryResolve(dispatcher.Object);
                              };

        It should_be_compile = () =>
                               {
                                   var render = new TemplateHandlebarsOnServerSide(htmlHelper)
                                           .Render(query.PathToView, query.Model, data);
                                   render.ShouldEqual(@"<option  value=""1"" title="""">1</option><option  value=""2"" title="""">2</option>");
                               };

        It should_be_compile_wihtout_view_model = () =>
                                                  {
                                                      query.Model = null;
                                                      var render = new TemplateHandlebarsOnServerSide(htmlHelper)
                                                              .Render(query.PathToView, data);
                                                      render.ShouldEqual(@"<option  value=""1"" title="""">1</option><option  value=""2"" title="""">2</option>");
                                                  };

        It should_be_compile_performance = () =>
                                           {
                                               var handlebarsOnServerSide = new TemplateHandlebarsOnServerSide(htmlHelper);

                                               Pleasure.Do(i => handlebarsOnServerSide
                                                                        .Render(query.PathToView, query.Model, data)
                                                                        .ShouldNotBeEmpty(), 1000).ShouldBeLessThan(50);
                                           };
    }
}