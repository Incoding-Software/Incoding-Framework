namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using Incoding.MSpecContrib;
    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(IncHiddenControl<,>))]
    public class When_inc_checkbox_control_unchecked : Context_inc_control
    {
        Because of = () =>
                         {
                             result = new IncodingHtmlHelperFor<FakeModel, object>(mockHtmlHelper.Original, r => r.Prop)
                                     .Checkbox(boxControl => { boxControl.Label.Name = Pleasure.Generator.TheSameString(); });
                         };

        It should_be_render = () =>
                              result.ToString()
                                    .ShouldEqual("<label class=\"checkbox\" for=\"Prop\"><input id=\"Prop\" name=\"Prop\" type=\"checkbox\" value=\"true\" /><input name=\"Prop\" type=\"hidden\" value=\"false\" />TheSameString</label>");
    }
}