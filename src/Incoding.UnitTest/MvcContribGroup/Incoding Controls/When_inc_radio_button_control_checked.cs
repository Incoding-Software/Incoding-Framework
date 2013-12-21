namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(IncHiddenControl<,>))]
    public class When_inc_radio_button_control_checked : Context_inc_control
    {
        Establish establish = () => mockHtmlHelper.StubModel(new FakeModel { Prop = "Male" });

        Because of = () =>
                         {
                             result = new IncodingHtmlHelperFor<FakeModel, object>(mockHtmlHelper.Original, r => r.Prop)
                                     .RadioButton(boxControl => { boxControl.Value = "Male"; });
                         };

        It should_be_render = () => result.ToString()
                                          .ShouldEqual("<label class=\"btn btn-default\" for=\"Prop\"><input checked=\"checked\" id=\"Prop\" name=\"Prop\" type=\"radio\" value=\"Male\" />Male</label>");
    }
}