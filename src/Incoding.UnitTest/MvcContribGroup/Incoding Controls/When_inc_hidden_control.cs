namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(IncHiddenControl<,>))]
    public class When_inc_hidden_control : Context_inc_control
    {
        #region Estabilish value

        static string result;

        #endregion

        Because of = () =>
                         {
                             result = new IncodingHtmlHelperFor<FakeModel, object>(mockHtmlHelper.Original, r => r.Prop)
                                     .Hidden()
                                     .ToHtmlString();
                         };

        It should_be_render = () => result.ShouldEqual("<input id=\"Prop\" name=\"Prop\" type=\"hidden\" value=\"TheSameString\" />");
    }
}