namespace Incoding.UnitTest.MvcContribGroup
{
    using Incoding.MvcContrib;
    using Machine.Specifications;

    [Subject(typeof(IncHiddenControl<,>))]
    public class When_inc_radio_button_control_with_null_model : Context_inc_control
    {
        Establish establish = () => mockHtmlHelper.StubModel(null);

        Because of = () =>
                     {
                         result = new IncodingHtmlHelperFor<FakeModel, object>(mockHtmlHelper.Original, r => r.Prop)
                                 .RadioButton(boxControl => { boxControl.Value = "Male"; });
                     };

        It should_be_render = () => result.ToString()
                                          .ShouldEqual("<div class=\"radio\"><label><input checked=\"checked\" id=\"Prop\" name=\"Prop\" type=\"radio\" value=\"Male\" /><i></i><span>Male</span></label></div>");
    }
}