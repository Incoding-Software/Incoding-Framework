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
        #region Estabilish value

        static string result;

        #endregion

        Because of = () =>
                         {
                             result = new IncodingHtmlHelperFor<FakeModel, object>(mockHtmlHelper.Original, r => r.Prop)
                                     .Checkbox(boxControl => { boxControl.Name = Pleasure.Generator.TheSameString(); })
                                     .ToHtmlString();
                         };

        It should_be_render = () =>
                              result.ShouldEqual("<label class=\"checkbox\"><input id=\"Prop\" name=\"Prop\" type=\"checkbox\" value=\"true\" /><input name=\"Prop\" type=\"hidden\" value=\"false\" /> TheSameString</label>");
    }
}