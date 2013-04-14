namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System.Web.Mvc;
    using Incoding.MvcContrib;
    using Machine.Specifications;using Incoding.MSpecContrib;

    #endregion

    [Subject(typeof(IncDropDownControl<,>))]
    public class When_inc_list_box_control : Context_inc_control
    {
        #region Estabilish value

        static string result;

        #endregion

        Because of = () =>
                         {
                             result = new IncodingHtmlHelperFor<FakeModel, object>(mockHtmlHelper.Original, r => r.Prop)
                                     .ListBox(boxControl =>
                                                  {
                                                      boxControl.Data = new SelectList(new[] { Pleasure.Generator.TheSameString() });
                                                      boxControl.Optional = "Optional";
                                                  })
                                     .ToHtmlString();
                         };

        It should_be_render = () => result.ShouldEqual("<select id=\"Prop\" multiple=\"multiple\" name=\"Prop\"><option value=\"\">Optional</option>\r\n<option selected=\"selected\">TheSameString</option>\r\n</select>");
    }
}