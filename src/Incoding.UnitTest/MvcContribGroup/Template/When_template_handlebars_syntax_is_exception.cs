namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(TemplateHandlebarsSyntax<>))]
    public class When_template_handlebars_syntax_is_exception : Context_template
    {
        It should_be_is_only_member_access = () => Catch.Exception(() => new TemplateHandlebarsSyntax<FakeModel>(htmlHelper.Original, "Data", HandlebarsType.If, string.Empty)
                                                                                 .Is(model => !model.Is))
                                                        .Message.ShouldContain(Resources.Exception_Handlerbars_Only_Member_Access);

        It should_be_not_only_member_access = () => Catch.Exception(() => new TemplateHandlebarsSyntax<FakeModel>(htmlHelper.Original, "Data", HandlebarsType.If, string.Empty)
                                                                                  .Not(model => !model.Is))
                                                         .Message.ShouldContain(Resources.Exception_Handlerbars_Only_Member_Access);
    }
}