namespace Incoding.UnitTest.MvcContribGroup
{
    using Incoding.MSpecContrib;
    using Incoding.MvcContrib;
    using Machine.Specifications;

    [Subject(typeof(IncHiddenControl<,>))]
    public class When_inc_checkbox_control_checked_by_attr : Context_inc_control
    {
        Establish establish = () => mockHtmlHelper.StubModel(new FakeModel { StringArrays = new string[0] });

        Because of = () =>  
                     {
                         result = new IncodingHtmlHelperFor<FakeModel, object>(mockHtmlHelper.Original, r => r.StringArrays)
                                 .CheckBox(boxControl =>
                                           {
                                               boxControl.Label.Name = Pleasure.Generator.TheSameString();
                                               boxControl.SetAttr(HtmlAttribute.Checked);

                                           });
                     };

        It should_be_render = () => result.ToString()
                                          .ShouldEqual("<div class=\" checkbox\"><label><input checked=\"checked\" id=\"StringArrays\" name=\"StringArrays\" type=\"checkbox\" value=\"true\" /><input name=\"StringArrays\" type=\"hidden\" value=\"false\" /><i></i><span>TheSameString</span></label></div>");
    }
}