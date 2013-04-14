namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using Incoding.MvcContrib;
    using Machine.Specifications;using Incoding.MSpecContrib;

    #endregion

    [Subject(typeof(IncTextBoxControl<,>))]
    public class When_inc_password_control : Context_inc_control
    {
        #region Estabilish value

        static string result;

        #endregion

        Because of = () =>
                         {
                             result = new IncodingHtmlHelperFor<FakeModel, object>(mockHtmlHelper.Original, r => r.Prop)
                                     .Password()
                                     .ToHtmlString();
                         };

        It should_be_render = () => result.ShouldEqual("<input id=\"Prop\" name=\"Prop\" type=\"password\" />");
    }
}