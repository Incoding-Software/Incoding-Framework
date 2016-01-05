namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(IncTextBoxControl<,>))]
    public class When_inc_text_box_control : Context_inc_control
    {
        #region Establish value

        static string result;

        #endregion

        It should_be_autocomplete_true = () => new IncodingHtmlHelperFor<FakeModel, object>(mockHtmlHelper.Original, r => r.Prop)
                                                       .TextBox(boxControl => { boxControl.Autocomplete = true; })
                                                       .ToHtmlString()
                                                       .ShouldEqual("<input autocomplete=\"autocomplete\" id=\"Prop\" name=\"Prop\" type=\"text\" value=\"TheSameString\" />");

        It should_be_full = () => new IncodingHtmlHelperFor<FakeModel, object>(mockHtmlHelper.Original, r => r.Prop)
                                          .TextBox(boxControl =>
                                                   {
                                                       boxControl.Placeholder = "placeholder";
                                                       boxControl.TabIndex = 5;
                                                       boxControl.ReadOnly = true;
                                                       boxControl.Disabled = true;
                                                       boxControl.Title = "title";
                                                       boxControl.MaxLength = 10;
                                                       boxControl.AddClass("class");
                                                       boxControl.AddClass("class2");
                                                       boxControl.Autocomplete = false;
                                                   })
                                          .ToHtmlString()
                                          .ShouldEqual("<input class=\"class class2\" disabled=\"disabled\" id=\"Prop\" maxlength=\"10\" name=\"Prop\" placeholder=\"placeholder\" readonly=\"readonly\" tabindex=\"5\" title=\"title\" type=\"text\" value=\"TheSameString\" />");
    }
}