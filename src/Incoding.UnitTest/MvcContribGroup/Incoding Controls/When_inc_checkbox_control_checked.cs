namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using Incoding.MSpecContrib;
    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(IncHiddenControl<,>))]
    public class When_inc_checkbox_control_checked : Context_inc_control
    {
        Establish establish = () => mockHtmlHelper.StubModel(new FakeModel { Prop = true.ToString() });

        Because of = () =>
                         {
                             result = new IncodingHtmlHelperFor<FakeModel, object>(mockHtmlHelper.Original, r => r.Prop)
                                     .CheckBox(boxControl =>
                                                   {
                                                       boxControl.Label.Name = Pleasure.Generator.TheSameString();
                                                       boxControl.Label.AddClass("special");
                                                   });
                         };

        It should_be_render = () => result.ToString()
                                          .ShouldEqual("<div class=\" checkbox\"><label><input checked=\"checked\" id=\"Prop\" name=\"Prop\" type=\"checkbox\" value=\"true\" /><input name=\"Prop\" type=\"hidden\" value=\"false\" /><i></i><span>TheSameString</span></label></div>");
    }
}