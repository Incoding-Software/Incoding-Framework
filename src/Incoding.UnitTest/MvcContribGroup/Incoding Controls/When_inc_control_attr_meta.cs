namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Routing;
    using Incoding.Extensions;
    using Incoding.MvcContrib;
    using Machine.Specifications;
    using Newtonsoft.Json.Linq;

    #endregion

    [Subject(typeof(IncTextBoxControl<,>))]
    public class When_inc_control_attr_meta : Context_inc_control
    {
        #region Establish value

        static IncTextBoxControl<FakeModel, string> control;

        static List<dynamic> metaFromControl;

        #endregion

        Establish establish = () =>
                                  {
                                      control = new IncTextBoxControl<FakeModel, string>(mockHtmlHelper.Original, r => r.Prop);
                                      control.Attr(new IncodingMetaLanguageDsl(JqueryBind.Click.ToString()).Do().Direct().AsHtmlAttributes());
                                      control.Attr(new IncodingMetaLanguageDsl(JqueryBind.Change.ToString()).Do().Event().AsHtmlAttributes());
                                  };

        Because of = () =>
                         {
                             metaFromControl = ((control.TryGetValue("attributes") as RouteValueDictionary)["incoding"].ToString().DeserializeFromJson<object>() as JContainer)
                                     .Cast<dynamic>()
                                     .ToList();
                         };

        It should_be_add_click = () => metaFromControl
                                               .Where(r => r.action.Value == "ActionDirect")
                                               .ShouldNotBeNull();

        It should_be_add_change = () => metaFromControl
                                                .Where(r => r.action.Value == "ActionEvent")
                                                .ShouldNotBeNull();

        It should_be_all = () => metaFromControl.Count.ShouldEqual(2);
    }
}