namespace Incoding.UnitTest.MvcContribGroup
{
    using Incoding.MvcContrib;
    using Machine.Specifications;

    [Subject(typeof(TemplateHandlebarsSyntax<>))]
    public class When_template_handlebars_syntax_is_bool : Context_template
    {
        Because of = () =>
                     {
                         var each = new TemplateHandlebarsSyntax<FakeModel>(htmlHelper.Original, "Data", HandlebarsType.If, string.Empty);
                         var newEach = each.Is(r => r.Is);
                         newEach.Dispose();
                     };

        It should_be_writer_start_not = () => htmlHelper.ShouldBeWriter("{{#if Is}}");

        It should_be_writer_end_not = () => htmlHelper.ShouldBeWriter("{{/if}}");
    }
}