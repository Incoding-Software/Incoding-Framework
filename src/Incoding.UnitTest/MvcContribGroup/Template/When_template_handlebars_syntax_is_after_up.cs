namespace Incoding.UnitTest.MvcContribGroup
{
    using Incoding.MvcContrib;
    using Machine.Specifications;

    [Subject(typeof(TemplateHandlebarsSyntax<>))]
    public class When_template_handlebars_syntax_is_after_up : Context_template
    {
        Because of = () =>
                         {
                             var each = new TemplateHandlebarsSyntax<FakeModel>(htmlHelper.Original, "Data", HandlebarsType.If, string.Empty);
                             each.Up().Is(r => r.Name);
                             var newEach = each.Is(model => model.Name);
                             newEach.Dispose();
                         };

        It should_be_writer_start_not = () => htmlHelper.ShouldBeWriter("{{#if Name}}");

        It should_be_writer_end_not = () => htmlHelper.ShouldBeWriter("{{/if}}");
    }
}