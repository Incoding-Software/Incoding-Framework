namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using Incoding.MvcContrib;
    using Machine.Specifications;using Incoding.MSpecContrib;

    #endregion

    [Subject(typeof(IncTextBoxControl<,>))]
    public class When_inc_text_box_control : Context_inc_control
    {
        #region Estabilish value

        static string result;

        #endregion

        Because of = () =>
                         {
                             result = new IncodingHtmlHelperFor<FakeModel, object>(mockHtmlHelper.Original, r => r.Prop)
                                     .TextBox(boxControl =>
                                                  {
                                                      boxControl.Placeholder = "placeholder";
                                                      boxControl.TabIndex = 5;
                                                      boxControl.AddClass("class");
                                                      boxControl.AddClass("class2");
                                                      boxControl.DisableAutoComplete();
                                                  })
                                     .ToHtmlString();
                         };

        It should_be_render = () => result.ShouldEqual("<input autocomplete=\"off\" class=\"class class2\" id=\"Prop\" name=\"Prop\" placeholder=\"placeholder\" tabindex=\"5\" type=\"text\" value=\"TheSameString\" />");
    }
}