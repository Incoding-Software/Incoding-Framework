namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(TemplateMustacheSyntax<>))]
    public class When_template_mustache_syntax_foreach : Context_template_mustache
    {
        #region Estabilish value

        static ITemplateSyntax<FakeModel> newEach;

        #endregion

        Because of = () =>
                         {
                             newEach = new TemplateMustacheSyntax<FakeModel>(htmlHelper.Original, "Data", true).ForEach(r => r.Items);
                             newEach.Dispose();
                         };

        It should_be_writer_start_not = () => htmlHelper.ShouldBeWriter("{{#Items}}");

        It should_be_writer_end_not = () => htmlHelper.ShouldBeWriter("{{/Items}}");

        It should_be_new_each_for = () => newEach.For(r => r.Name).ShouldEqual("{{Name}}");
    }
}