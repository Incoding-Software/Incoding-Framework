namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(TemplateMustacheSyntax<>))]
    public class When_template_mustache_syntax_not_each : Context_template
    {
        Because of = () =>
                         {
                             var each = new TemplateMustacheSyntax<FakeModel>(htmlHelper.Original, "data", false);
                             each.Dispose();
                         };

        It should_be_write_start = () => htmlHelper.ShouldBeWriter("{{^data}}");

        It should_be_write_end = () => htmlHelper.ShouldBeWriter("{{/data}}");
    }
}