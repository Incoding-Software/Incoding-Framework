namespace Incoding.UnitTest.MvcContribGroup
{
    using Incoding.MvcContrib;
    using Machine.Specifications;

    [Subject(typeof(TemplateHandlebarsSyntax<>))]
    public class When_template_handlebars_syntax_not_after_up : Context_template
    {
        Because of = () =>
                         {
                             var each = new TemplateHandlebarsSyntax<FakeModel>(htmlHelper.Original, "Data", HandlebarsType.Unless, string.Empty);
                             each.Up().Not(r => r.Name);
                             var newEach = each.Not(r => r.Name);
                             newEach.Dispose();
                         };

        It should_be_writer_start_not = () => htmlHelper.ShouldBeWriter("{{#unless Name}}");

        It should_be_writer_end_not = () => htmlHelper.ShouldBeWriter("{{/unless}}");
    }
}