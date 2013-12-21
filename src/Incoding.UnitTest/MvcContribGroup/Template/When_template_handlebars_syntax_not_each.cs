namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(TemplateHandlebarsSyntax<>))]
    public class When_template_handlebars_syntax_not_each : Context_template
    {
        Because of = () =>
                         {
                             var each = new TemplateHandlebarsSyntax<FakeModel>(htmlHelper.Original, "data", HandlebarsType.Unless, string.Empty);
                             each.Dispose();
                         };

        It should_be_write_start = () => htmlHelper.ShouldBeWriter("{{#unless data}}");

        It should_be_write_end = () => htmlHelper.ShouldBeWriter("{{/unless}}");
    }
}