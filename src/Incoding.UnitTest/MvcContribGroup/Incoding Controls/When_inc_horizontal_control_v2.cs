namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System;
    using System.Linq.Expressions;
    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(IncHiddenControl<,>))]
    public class When_inc_horizontal_control_v2 : Context_inc_control
    {
        #region Establish value

        static IncHorizontalControl<IncHiddenControl<FakeModel, object>> control;

        #endregion

        Establish establish = () =>
                              {
                                  IncodingHtmlHelper.BootstrapVersion =BootstrapOfVersion.v2;
                                  Expression<Func<FakeModel, object>> expression = model => model.Prop;
                                  var label = new IncLabelControl(mockHtmlHelper.Original, expression);
                                  var input = new IncHiddenControl<FakeModel, object>(mockHtmlHelper.Original, expression);
                                  var validation = new IncValidationControl(mockHtmlHelper.Original, expression);
                                  control = new IncHorizontalControl<IncHiddenControl<FakeModel, object>>(label, input, validation);
                              };

        Because of = () => { result = control.ToHtmlString(); };

        It should_be_render = () => result.ToString()
                                          .ShouldEqual("<div class=\"control-group\"><label class=\"control-label\" for=\"Prop\">Prop</label><div class=\"controls\"><input id=\"Prop\" name=\"Prop\" type=\"hidden\" value=\"TheSameString\" /></div></div>");
    }
}