namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System.Web.WebPages;
    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(TemplateMustacheSyntax<>))]
    public class When_template_mustache_syntax_positive : Context_template_mustache
    {
        #region Estabilish value

        static TemplateMustacheSyntax<FakeModel> each;

        #endregion

        Because of = () =>
                         {
                             each = new TemplateMustacheSyntax<FakeModel>(htmlHelper.Original, "data", true);
                             each.Dispose();
                         };

        It should_be_write_start = () => htmlHelper.ShouldBeWriter("{{#data}}");

        It should_be_write_end = () => htmlHelper.ShouldBeWriter("{{/data}}");

        It should_be_for = () => each.For(r => r.Name).ShouldEqual("{{Name}}");

        It should_be_for_is_inline = () => each.IsInline(r => r.Name, o => new HelperResult(writer => writer.Write("<b>if true</b>"))).ShouldEqual("{{#Name}}<b>if true</b>{{/Name}}");

        It should_be_for_not_inline = () => each.NotInline(r => r.Name, o => new HelperResult(writer => writer.Write("<b>if false</b>"))).ShouldEqual("{{^Name}}<b>if false</b>{{/Name}}");

        It should_be_for_raw = () => each.ForRaw(r => r.Name).ShouldEqual("{{{Name}}}");

        It should_be_for_raw_component = () => each.ForRaw(r => r.Fake.Name).ShouldEqual("{{{Fake.Name}}}");

        It should_be_for_component = () => each.For(r => r.Fake.Name).ShouldEqual("{{Fake.Name}}");
    }
}