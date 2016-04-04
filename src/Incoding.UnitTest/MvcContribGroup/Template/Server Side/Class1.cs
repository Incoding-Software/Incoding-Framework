namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System.Web.Mvc;
    using System.Web.Routing;
    using Incoding.MSpecContrib;
    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(TemplateHandlebarsOnServerSide))]
    public class When_template_handlebars
    {
        It should_be_compile = () =>
                               {
                                   var viewDataContainer = Pleasure.MockStrictAsObject<IViewDataContainer>(mock =>
                                                                                                           {
                                                                                                               mock.SetupGet(r => r.ViewData).Returns(new ViewDataDictionary());
                                                                                                           });
                                   new TemplateHandlebarsOnServerSide(new HtmlHelper(new ViewContext(), viewDataContainer, new RouteCollection()))
                                           .Render("test", new KeyValueVm(1))
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