namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System.Collections.Generic;
    using System.Web.Mvc;
    using Incoding.MSpecContrib;
    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(IncDropDownControl<,>))]
    public class When_inc_drop_down_control : Context_inc_control
    {
        Because of = () =>
                         {
                             result = new IncodingHtmlHelperFor<FakeModel, object>(mockHtmlHelper.Original, r => r.Prop)
                                     .DropDown(boxControl =>
                                                   {
                                                       boxControl.Data = new SelectList(new[] { Pleasure.Generator.TheSameString() });
                                                       boxControl.Optional = new List<KeyValueVm>
                                                                                 {
                                                                                         new KeyValueVm("Optional")
                                                                                 };
                                                   });
                         };

        It should_be_render = () => result.ToString()
                                          .ShouldEqual("<select id=\"Prop\" name=\"Prop\"><option value=\"Optional\">Optional</option>\r\n<option selected=\"selected\" value=\"TheSameString\">TheSameString</option>\r\n</select>");
    }
}