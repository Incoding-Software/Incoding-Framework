namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(IncTextBoxControl<,>))]
    public class When_inc_text_area_control : Context_inc_control
    {
        #region Estabilish value

        static string result;

        #endregion

        Because of = () =>
                         {
                             result = new IncodingHtmlHelperFor<FakeModel, object>(mockHtmlHelper.Original, r => r.Prop)
                                     .TextArea(boxControl =>
                                                   {
                                                       boxControl.Cols = 10;
                                                       boxControl.Rows = 30;
                                                   })
                                     .ToHtmlString();
                         };

        It should_be_render = () => result.ShouldEqual("<textarea cols=\"10\" id=\"Prop\" name=\"Prop\" rows=\"30\">\r\nTheSameString</textarea>");
    }
}