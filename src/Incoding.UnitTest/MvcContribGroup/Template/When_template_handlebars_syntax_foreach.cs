namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(TemplateHandlebarsSyntax<>))]
    public class When_template_handlebars_syntax_foreach : Context_template
    {
        #region Establish value

        static ITemplateSyntax<FakeModel> newEach;

        #endregion

        Because of = () =>
                         {
                             var syntax = new TemplateHandlebarsSyntax<FakeModel>(htmlHelper.Original, "Data", HandlebarsType.Each, string.Empty);
                             newEach = syntax.ForEach(r => r.Items);
                             newEach.Dispose();
                         };

        It should_be_writer_start_not = () => htmlHelper.ShouldBeWriter("{{#each Items}}");

        It should_be_writer_end_not = () => htmlHelper.ShouldBeWriter("{{/each}}");

        It should_be_new_each_for = () => newEach.For(r => r.Name).ShouldEqual("{{Name}}");
    }
}