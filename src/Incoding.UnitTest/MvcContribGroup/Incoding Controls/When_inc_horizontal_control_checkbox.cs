namespace Incoding.UnitTest.MvcContribGroup
{
    using System;
    using System.Linq.Expressions;
    using Incoding.MvcContrib;
    using Machine.Specifications;

    [Subject(typeof(IncHiddenControl<,>))]
    public class When_inc_horizontal_control_v3_checkbox : Context_inc_control
    {
        #region Establish value

        static IncHorizontalControl<IncCheckBoxControl<FakeModel, object>> control;

        #endregion

        Establish establish = () =>
                              {
                                  IncodingHtmlHelper.BootstrapVersion = BootstrapOfVersion.v3;
                                  Expression<Func<FakeModel, object>> expression = model => model.Prop;
                                  var label = new IncLabelControl(mockHtmlHelper.Original, expression);
                                  var input = new IncCheckBoxControl<FakeModel, object>(mockHtmlHelper.Original, expression);
                                  var validation = new IncValidationControl(mockHtmlHelper.Original, expression);
                                  control = new IncHorizontalControl<IncCheckBoxControl<FakeModel, object>>(label, input, validation);
                                  
                              };

        Because of = () => { result = control.ToHtmlString(); };

        It should_be_render = () => result.ToString()
                                          .ShouldEqual("<div class=\"col-xs-12 form-group\"><label class=\"col-xs-5 control-label\" for=\"Prop\">Prop</label><div class=\"col-xs-7\"><input class=\"col-xs-7 form-control\" id=\"Prop\" name=\"Prop\" type=\"hidden\" value=\"TheSameString\" /></div></div>");

        Cleanup clean = () => IncodingHtmlHelper.BootstrapVersion = BootstrapOfVersion.v2;
    }
}