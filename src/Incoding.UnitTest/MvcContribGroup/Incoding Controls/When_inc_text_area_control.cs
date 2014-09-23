namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(IncTextBoxControl<,>))]
    public class When_inc_text_area_control : Context_inc_control
    {
        #region Establish value

        static string result;

        #endregion

        Because of = () =>
                         {
                             result = new IncodingHtmlHelperFor<FakeModel, object>(mockHtmlHelper.Original, r => r.Prop)
                                     .TextArea(boxControl =>
                                                   {
                                                       boxControl.Cols = 10;
                                                       boxControl.Rows = 30;
                                                       boxControl.MaxLenght = 15;
                                                   })
                                     .ToHtmlString();
                         };

        It should_be_render = () => result.ShouldEqual(@"<textarea cols=""10"" id=""Prop"" maxlength=""15"" name=""Prop"" rows=""30"">
TheSameString</textarea>");
    }
}