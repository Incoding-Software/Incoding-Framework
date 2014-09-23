namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(IncTextBoxControl<,>))]
    public class When_inc_password_control : Context_inc_control
    {
        Because of = () =>
                         {
                             result = new IncodingHtmlHelperFor<FakeModel, object>(mockHtmlHelper.Original, r => r.Prop)
                                     .Password(control => { control.MaxLenght = 10; });
                         };

        It should_be_render = () => result.ToString().ShouldEqual("<input id=\"Prop\" maxlength=\"10\" name=\"Prop\" type=\"password\" />");
    }
}