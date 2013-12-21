namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(TemplateHandlebarsSyntax<>))]
    public class When_template_handlebars_syntax_is_with_up : Context_template
    {
        Because of = () =>
                         {
                             var each = new TemplateHandlebarsSyntax<FakeModel>(htmlHelper.Original, "Data", HandlebarsType.If, string.Empty);
                             var newEach = each.Up().Is(r => r.Name);
                             newEach.Dispose();
                         };

        It should_be_writer_start_not = () => htmlHelper.ShouldBeWriter("{{#if ../Name}}");

        It should_be_writer_end_not = () => htmlHelper.ShouldBeWriter("{{/if}}");
    }
}