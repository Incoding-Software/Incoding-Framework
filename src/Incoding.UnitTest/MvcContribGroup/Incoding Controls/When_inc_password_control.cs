namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(IncPasswordControl<,>))]
    public class When_inc_password_control : Context_inc_control
    {
        Because of = () =>
                     {
                         result = new IncodingHtmlHelperFor<FakeModel, object>(mockHtmlHelper.Original, r => r.Prop)
                                 .Password(control =>
                                           {
                                               control.MaxLength = 10;
                                               control.Placeholder = "Placeholder";
                                           });
                     };

        It should_be_render = () => result.ToString().ShouldEqual("<input id=\"Prop\" maxlength=\"10\" name=\"Prop\" placeholder=\"Placeholder\" type=\"password\" />");
    }
}