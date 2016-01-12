namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using Incoding.MSpecContrib;
    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(IncHiddenControl<,>))]
    public class When_inc_radio_button_control_with_name : Context_inc_control
    {
        Because of = () =>
                     {
                         result = new IncodingHtmlHelperFor<FakeModel, object>(mockHtmlHelper.Original, r => r.Prop)
                                 .RadioButton(boxControl =>
                                              {
                                                  boxControl.Label.Name = Pleasure.Generator.TheSameString();
                                                  boxControl.Value = "Male";
                                                  boxControl.AddClass(B.Hidden);
                                              });
                     };

        It should_be_render = () => result.ToString()
                                          .ShouldEqual("<div class=\"hidden radio\"><label><input class=\"hidden\" id=\"Prop\" name=\"Prop\" type=\"radio\" value=\"Male\" /><i></i><span>TheSameString</span></label></div>");
    }
}