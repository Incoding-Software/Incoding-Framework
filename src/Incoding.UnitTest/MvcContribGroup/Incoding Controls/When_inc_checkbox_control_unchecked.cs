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
                                     .CheckBox(boxControl => { boxControl.Label.Name = Pleasure.Generator.TheSameString(); });
                         };

        It should_be_render = () =>
                              result.ToString()
                                    .ShouldEqual(@"<div class="" checkbox""><label><input id=""Prop"" name=""Prop"" type=""checkbox"" value=""true"" /><input name=""Prop"" type=""hidden"" value=""false"" /><i></i><span>TheSameString</span></label></div>");
    }
}