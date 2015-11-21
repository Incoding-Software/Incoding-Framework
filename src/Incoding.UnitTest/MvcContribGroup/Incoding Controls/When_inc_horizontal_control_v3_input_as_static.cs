namespace Incoding.UnitTest.MvcContribGroup
{
    using System;
    using System.Linq.Expressions;
    using Incoding.MvcContrib;
    using Machine.Specifications;

    [Subject(typeof(IncHiddenControl<,>))]
    public class When_inc_horizontal_control_v3_input_as_static : Context_inc_control
    {
        #region Establish value

        static IncHorizontalControl<IncStaticControl<FakeModel, object>> control;

        #endregion

        Establish establish = () =>
                              {
                                  IncodingHtmlHelper.BootstrapVersion = BootstrapOfVersion.v3;
                                  Expression<Func<FakeModel, object>> expression = model => model.Prop;
                                  var label = new IncLabelControl(mockHtmlHelper.Original, expression);
                                  var input = new IncStaticControl<FakeModel, object>(mockHtmlHelper.Original, expression);
                                  var validation = new IncValidationControl(mockHtmlHelper.Original, expression);
                                  control = new IncHorizontalControl<IncStaticControl<FakeModel, object>>(label, input, validation);                                  
                              };

        Because of = () => { result = control.ToHtmlString(); };

        It should_be_render = () => result.ToString()
                                          .ShouldEqual("<div class=\"form-group\"><label class=\"control-label col-xs-2\" for=\"Prop\">Prop</label><div class=\"col-xs-10\"><p class=\"form-control-static\">TheSameString</p></div></div>");

        Cleanup clean = () => IncodingHtmlHelper.BootstrapVersion = BootstrapOfVersion.v2;
    }
}