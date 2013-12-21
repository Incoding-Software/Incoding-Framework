namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System.Web.Mvc;
    using Incoding.MSpecContrib;
    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(IncDropDownControl<,>))]
    public class When_inc_list_box_control : Context_inc_control
    {
        Because of = () =>
                         {
                             result = new IncodingHtmlHelperFor<FakeModel, object>(mockHtmlHelper.Original, r => r.Prop)
                                     .ListBox(boxControl =>
                                                  {
                                                      boxControl.Data = new SelectList(new[] { Pleasure.Generator.TheSameString() });
                                                      boxControl.Optional = new[] { new KeyValueVm("Optional") };
                                                  });
                         };

        It should_be_render = () => result.ToString()
                                          .ShouldEqual("<select id=\"Prop\" multiple=\"multiple\" name=\"Prop\"><option value=\"\">Optional</option>\r\n<option selected=\"selected\">TheSameString</option>\r\n</select>");
    }
}