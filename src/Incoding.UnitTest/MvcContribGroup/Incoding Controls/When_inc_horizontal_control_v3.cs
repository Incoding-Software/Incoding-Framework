namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System;
    using System.Linq.Expressions;
    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(IncHiddenControl<,>))]
    public class When_inc_horizontal_control_v3 : Context_inc_control
    {
        #region Establish value

        static IncHorizontalControl<IncHiddenControl<FakeModel, object>> control;

        #endregion

        Establish establish = () =>
                              {
                                  IncodingHtmlHelper.BootstrapVersion = BootstrapOfVersion.v3;
                                  Expression<Func<FakeModel, object>> expression = model => model.Prop;
                                  var label = new IncLabelControl(mockHtmlHelper.Original, expression);
                                  var input = new IncHiddenControl<FakeModel, object>(mockHtmlHelper.Original, expression);
                                  var validation = new IncValidationControl(mockHtmlHelper.Original, expression);
                                  control = new IncHorizontalControl<IncHiddenControl<FakeModel, object>>(label, input, validation);
                                  control.Label.AddClass(B.Col_xs_5);
                                  control.Input.AddClass(B.Col_xs_7);
                                  control.Control.AddClass(B.Col_xs_7);
                                  control.AddClass(B.Col_xs_12);
                              };

        Because of = () => { result = control.ToHtmlString(); };

        It should_be_render = () => result.ToString()
                                          .ShouldEqual("<div class=\"col-xs-12 form-group\"><label class=\"col-xs-5 control-label\" for=\"Prop\">Prop</label><div class=\"col-xs-7\"><input class=\"col-xs-7 form-control\" id=\"Prop\" name=\"Prop\" type=\"hidden\" value=\"TheSameString\" /></div></div>");

        Cleanup clean = () => IncodingHtmlHelper.BootstrapVersion = BootstrapOfVersion.v2;
    }
}