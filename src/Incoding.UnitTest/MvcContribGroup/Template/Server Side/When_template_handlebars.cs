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
        It should_be_compile = () =>
                               {
                                   var htmlHelper = new HtmlHelper(new ViewContext(), Pleasure.MockStrictAsObject<IViewDataContainer>(), new RouteCollection());
                                   var dispatcher = Pleasure.Mock<IDispatcher>();
                                   var query = Pleasure.Generator.Invent<RenderViewQuery>(dsl => dsl.Tuning(r => r.HtmlHelper, htmlHelper));
                                   var tmpl = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "handlebars_sample_tmpl.txt"));
                                   dispatcher.StubQuery(query, new MvcHtmlString(tmpl));
                                   IoCFactory.Instance.StubTryResolve(dispatcher.Object);

                                   new TemplateHandlebarsOnServerSide(htmlHelper)
                                           .Render(query.PathToView, query.Model, new OptGroupVm(new List<KeyValueVm>()
                                                                                                 {
                                                                                                         new KeyValueVm(1),
                                                                                                         new KeyValueVm(2)
                                                                                                 }))
                                           .ShouldEqual("");
                               };

        It should_be_compile_from_cache = () =>
                                          {
                                              //typeof(TemplateHandlebarsOnServerSide)
                                              //.GetField("cache",BindingFlags.Static)
                                              //.SetValue(new Dictionary<string>);
                                          };
    }
}