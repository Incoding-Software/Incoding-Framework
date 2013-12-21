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
                                     .Checkbox(boxControl =>
                                                   {
                                                       boxControl.Label.Name = Pleasure.Generator.TheSameString();
                                                       boxControl.Label.AddClass("special");
                                                   });
                         };

        It should_be_render = () => result.ToString()
                                          .ShouldEqual("<label class=\"checkbox special\" for=\"Prop\"><input checked=\"checked\" id=\"Prop\" name=\"Prop\" type=\"checkbox\" value=\"true\" /><input name=\"Prop\" type=\"hidden\" value=\"false\" />TheSameString</label>");
    }
}