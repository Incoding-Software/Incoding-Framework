namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(TemplateHandlebarsSyntax<>))]
    public class When_template_handlebars_syntax_foreach_with_up : Context_template
    {
        #region Estabilish value

        static ITemplateSyntax<FakeModel> newEach;

        #endregion

        Because of = () =>
                         {
                             var syntax = new TemplateHandlebarsSyntax<FakeModel>(htmlHelper.Original, "Data", HandlebarsType.Each, string.Empty);
                             newEach = syntax.Up().ForEach(r => r.Items);
                             newEach.Dispose();
                         };

        It should_be_writer_start_not = () => htmlHelper.ShouldBeWriter("{{#each ../Items}}");

        It should_be_writer_end_not = () => htmlHelper.ShouldBeWriter("{{/each}}");
    }
}