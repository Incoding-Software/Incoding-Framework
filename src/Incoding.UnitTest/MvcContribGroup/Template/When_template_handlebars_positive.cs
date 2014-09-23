namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System.Web.Mvc;
    using System.Web.WebPages;
    using Incoding.Extensions;
    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(TemplateHandlebarsSyntax<>))]
    public class When_template_handlebars_positive : Context_template
    {
        #region Establish value

        static TemplateHandlebarsSyntax<FakeModel> each;

        const string etalonToIsInline = "{{#if Is}}true{{else}}false{{/if}}";

        #endregion

        Because of = () =>
                         {
                             each = new TemplateHandlebarsSyntax<FakeModel>(htmlHelper.Original, "data", HandlebarsType.Each, string.Empty);
                             each.Dispose();
                         };

        It should_be_write_start = () => htmlHelper.ShouldBeWriter("{{#each data}}");

        It should_be_write_end = () => htmlHelper.ShouldBeWriter("{{/each}}");

        It should_be_for_multiple = () =>
                                        {
                                            each.Up().For(r => r.Name).ShouldEqual("{{../Name}}");
                                            each.For(r => r.Name).ShouldEqual("{{Name}}");
                                        };

        It should_be_for = () => each.For(r => r.Name).ShouldEqual("{{Name}}");

        It should_be_for_bool = () => each.For(r => r.Is).ShouldEqual("{{#if Is}}true{{else}}false{{/if}}");

        It should_be_for_with_up = () => each.Up().For(r => r.Name).ShouldEqual("{{../Name}}");

        It should_be_inline_as_string_string = () => each.Inline(r => r.Is, "true", "false")
                                                         .ToHtmlString()
                                                         .ShouldEqual(etalonToIsInline);

        It should_be_inline_as_mvchtmlstring_string = () => each.Inline(r => r.Is, new MvcHtmlString("true"), "false")
                                                                .ToHtmlString()
                                                                .ShouldEqual(etalonToIsInline);

        It should_be_inline_as_string_mvchtmlstring = () => each.Inline(r => r.Is, "true", "false".ToMvcHtmlString())
                                                                .ToHtmlString()
                                                                .ShouldEqual(etalonToIsInline);

        It should_be_inline_as_mvchtmlstring_mvchtmlstring = () => each.Inline(r => r.Is, "true".ToMvcHtmlString(), "false".ToMvcHtmlString())
                                                                       .ToHtmlString()
                                                                       .ShouldEqual(etalonToIsInline);

        It should_be_inline_as_invoke_mvchtmlstring = () => each.Inline(r => r.Is, o => new HelperResult(writer => writer.Write("true")), "false".ToMvcHtmlString())
                                                                .ToHtmlString()
                                                                .ShouldEqual(etalonToIsInline);

        It should_be_inline_as_invoke_string = () => each.Inline(r => r.Is, o => new HelperResult(writer => writer.Write("true")), "false")
                                                         .ToHtmlString()
                                                         .ShouldEqual(etalonToIsInline);

        It should_be_inline_as_string_invoke = () => each.Inline(r => r.Is, "true", o => new HelperResult(writer => writer.Write("false")))
                                                         .ToHtmlString()
                                                         .ShouldEqual(etalonToIsInline);

        It should_be_inline_as_mvchtmlstring_invoke = () => each.Inline(r => r.Is, "true".ToMvcHtmlString(), o => new HelperResult(writer => writer.Write("false")))
                                                                .ToHtmlString()
                                                                .ShouldEqual(etalonToIsInline);

        It should_be_inline_as_invoke_invoke = () => each.Inline(r => r.Is, o => new HelperResult(writer => writer.Write("true")), o => new HelperResult(writer => writer.Write("false")))
                                                         .ToHtmlString()
                                                         .ShouldEqual(etalonToIsInline);

        It should_be_is_inline = () => each.IsInline(r => r.Name, o => new HelperResult(writer => writer.Write("<b>if true</b>"))).ToHtmlString().ShouldEqual("{{#if Name}}<b>if true</b>{{/if}}");

        It should_be_is_inline_as_string = () => each.IsInline(r => r.Name, new MvcHtmlString("<b>if true</b>")).ToHtmlString().ShouldEqual("{{#if Name}}<b>if true</b>{{/if}}");

        It should_be_is_inline_with_up = () => each.Up().IsInline(r => r.Name, "test").ToHtmlString().ShouldEqual("{{#if ../Name}}test{{/if}}");

        It should_be_not_inline = () => each.NotInline(r => r.Name, o => new HelperResult(writer => writer.Write("<b>if false</b>"))).ToHtmlString().ShouldEqual("{{#unless Name}}<b>if false</b>{{/unless}}");

        It should_be_not_inline_as_string = () => each.NotInline(r => r.Name, new MvcHtmlString("<b>if false</b>")).ToHtmlString().ShouldEqual("{{#unless Name}}<b>if false</b>{{/unless}}");

        It should_be_not_inline_with_up = () => each.Up().NotInline(r => r.Name, "test").ToHtmlString().ShouldEqual("{{#unless ../Name}}test{{/unless}}");

        It should_be_for_raw = () => each.ForRaw(r => r.Name).ToHtmlString().ShouldEqual("{{{Name}}}");

        It should_be_for_raw_component = () => each.ForRaw(r => r.Fake.Name).ToHtmlString().ShouldEqual("{{{Fake.Name}}}");

        It should_be_for_raw_with_up = () => each.Up().ForRaw(r => r.Name).ToHtmlString().ShouldEqual("{{{../Name}}}");

        It should_be_for_component = () => each.For(r => r.Fake.Name).ShouldEqual("{{Fake.Name}}");
    }
}