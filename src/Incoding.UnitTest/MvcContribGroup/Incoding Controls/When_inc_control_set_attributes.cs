namespace Incoding.UnitTest.MvcContribGroup
{
    using System.Collections.Generic;
    using System.Web.Routing;
    using Incoding.Extensions;
    using Incoding.MSpecContrib;
    using Incoding.MvcContrib;
    using Machine.Specifications;

    [Subject(typeof(IncControlBase))]
    public class When_inc_control_set_attributes : Context_inc_control
    {
        #region Establish value

        static IncTextBoxControl<FakeModel, string> control;

        static IDictionary<string, object> attr;

        #endregion

        Establish establish = () =>
                              {
                                  control = new IncTextBoxControl<FakeModel, string>(mockHtmlHelper.Original, r => r.Prop);                                  
                                  control.SetAttr(HtmlAttribute.Class, "test");
                                  control.SetAttr(HtmlAttribute.Checked);
                              };

        Because of = () => { attr = control.TryGetValue("attributes") as RouteValueDictionary; };

        It should_be_class = () => attr.ShouldBeKeyValue("class", "test");

        It should_be_checked = () => attr.ShouldBeKeyValue("checked", "checked");
    }
}