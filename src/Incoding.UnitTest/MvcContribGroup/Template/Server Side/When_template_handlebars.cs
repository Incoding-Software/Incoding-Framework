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
    using Moq;
    using It = Machine.Specifications.It;

    #endregion

    [Subject(typeof(TemplateHandlebarsOnServerSide))]
    public class When_template_handlebars
    {
        private static RenderViewQuery query;

        private static HtmlHelper htmlHelper;

        private static object data;

        private static Mock<IDispatcher> dispatcher;

        Establish establish = () =>
                              {
                                  data = new List<KeyValueVm>() { new KeyValueVm(1), new KeyValueVm(2) };
                                  htmlHelper = new HtmlHelper(new ViewContext(), Pleasure.MockStrictAsObject<IViewDataContainer>(), new RouteCollection());
                                  dispatcher = Pleasure.Mock<IDispatcher>();
                                  query = Pleasure.Generator.Invent<RenderViewQuery>(dsl => dsl.Tuning(r => r.HtmlHelper, htmlHelper));
                                  var tmpl = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "handlebars_sample_tmpl.txt"));
                                  dispatcher.StubQuery(query, new MvcHtmlString(tmpl));
                                  IoCFactory.Instance.StubTryResolve(dispatcher.Object);
                              };

        It should_be_compile = () =>
                               {
                                   new TemplateHandlebarsOnServerSide()
                                           .Render(htmlHelper, query.PathToView, data, query.Model)
                                           .ShouldEqual(@"<option  value=""1"" title="""">1</option><option  value=""2"" title="""">2</option>");
                               };

        It should_be_compile_performance = () =>
                                           {
                                               Pleasure.Do(i => new TemplateHandlebarsOnServerSide()
                                                                        .Render(htmlHelper, query.PathToView, data, query.Model)
                                                                        .ShouldNotBeEmpty(), 1000).ShouldBeLessThan(50);
                                           };

        It should_be_compile_wihtout_view_model = () =>
                                                  {
                                                      query.Model = null;
                                                      var render = new TemplateHandlebarsOnServerSide()
                                                              .Render(htmlHelper, query.PathToView, data);
                                                      render.ShouldEqual(@"<option  value=""1"" title="""">1</option><option  value=""2"" title="""">2</option>");
                                                  };

        It should_be_complexity = () =>
                                  {
                                      var tmpl = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "handlebars_complexity_tmpl.txt"));
                                      var newQuery = Pleasure.Generator.Invent<RenderViewQuery>(dsl => dsl.Tuning(r => r.HtmlHelper, htmlHelper));
                                      dispatcher.StubQuery(newQuery, new MvcHtmlString(tmpl));                                      
                                      var render = new TemplateHandlebarsOnServerSide()
                                              .Render(htmlHelper, query.PathToView, new { data = new ComplexityVm() }, query.Model);
                                      render.ShouldEqual(@"<option  value=""1"" title="""">1</option><option  value=""2"" title="""">2</option>");
                                  };

        public class ComplexityVm
        {
            public string Id { get; set; }

            public string Year { get; set; }

            public OfUser UW { get; set; }

            public List<OfCompany> Brokers { get; set; }

            public OfCompany Client { get; set; }

            public OfUser Analyst { get; set; }

            public string Description { get; set; }

            public string Incept { get; set; }

            public string Expiry { get; set; }

            public string Stamp { get; set; }

            public string Supergroup { get; set; }

            public string Class { get; set; }

            public string ContractType { get; set; }

            public string TreatyCoverageBasis { get; set; }

            public class OfUser
            {
                public string FullName { get; set; }

                public string Email { get; set; }
            }

            public class OfCompany
            {
                public string FullName { get; set; }

                public string Phone { get; set; }

                public string Code { get; set; }

                public string Address { get; set; }
            }
        }
    }
}