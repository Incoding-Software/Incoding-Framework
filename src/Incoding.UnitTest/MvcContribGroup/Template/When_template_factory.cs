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
    public class When_template_factory
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
                                  };

        It should_be_handlebars_for_each = () => new TemplateHandlebarsFactory().ForEach<FakeModel>(htmlHelper)
                                                                                .ShouldEqualWeak(new
                                                                                                     {
                                                                                                             property = "data", 
                                                                                                             level = string.Empty, 
                                                                                                             type = HandlebarsType.Each
                                                                                                     }, dsl => dsl
                                                                                                                       .IncludeAllFields()
                                                                                                                       .Ignore("htmlHelper", "design"));

        It should_be_handlebars_not_each = () => new TemplateHandlebarsFactory().NotEach<FakeModel>(htmlHelper)
                                                                                .ShouldEqualWeak(new
                                                                                                     {
                                                                                                             property = "data", 
                                                                                                             level = string.Empty, 
                                                                                                             type = HandlebarsType.Unless
                                                                                                     }, dsl => dsl
                                                                                                                       .IncludeAllFields()
                                                                                                                       .Ignore("htmlHelper", "design"));

        It should_be_mustache_for_each = () => new TemplateMustacheFactory().ForEach<FakeModel>(htmlHelper)
                                                                            .ShouldEqualWeak(new { property = "data", level = string.Empty }, dsl => dsl
                                                                                                                                                             .IncludeAllFields()
                                                                                                                                                             .Ignore("htmlHelper", "design"));

        It should_be_mustache_not_each = () => new TemplateMustacheFactory().NotEach<FakeModel>(htmlHelper)
                                                                            .ShouldEqualWeak(new { property = "data", level = string.Empty }, dsl => dsl
                                                                                                                                                             .IncludeAllFields()
                                                                                                                                                             .Ignore("htmlHelper", "design"));

        It should_be_mustache_get_drop_down_template = () => new TemplateMustacheFactory()
                                                                     .GetDropDownTemplate()
                                                                     .ShouldEqual(@"{{#data}}{{#Title}} <optgroup label=""{{Title}}"">
                               {{#Items}} 
                               <option {{#Selected}}selected=""selected""{{/Selected}} value=""{{Value}}"">{{Text}}</option>
                               {{/Items}} </optgroup>
                               {{/Title}}{{^Title}}
                               {{#Items}} <option {{#Selected}}selected=""selected""{{/Selected}} value=""{{Value}}"">{{Text}}</option>
                               {{/Items}}
                               {{/Title}}
                               {{/data}}");

        It should_be_handlebars_get_drop_down_template = () => new TemplateHandlebarsFactory()
                                                                       .GetDropDownTemplate()
                                                                       .ShouldEqual(@"{{#data}}
                                 {{#if Title}}
                                 <optgroup label=""{{Title}}"">
                                 {{#each Items}}
                                 <option {{#Selected}}selected=""selected""{{/Selected}} value=""{{Value}}"">{{Text}}</option>
                                 {{/each}}
                                 </optgroup>
                                 {{else}}
                                 {{#each Items}}
                                 <option {{#Selected}}selected=""selected""{{/Selected}} value=""{{Value}}"">{{Text}}</option>
                                 {{/each}}
                                 {{/if}}
                                 {{/data}}");
    }
}