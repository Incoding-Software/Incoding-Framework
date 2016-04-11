namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System;
    using System.Linq.Expressions;
    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(IncHiddenControl<,>))]
    public class When_inc_horizontal_control_v3_and_checkbox : Context_inc_control
    {
        #region Establish value

        static IncHorizontalControl<IncCheckBoxControl<FakeModel, object>> control;

        #endregion

        Cleanup clean = () => IncodingHtmlHelper.BootstrapVersion = BootstrapOfVersion.v2;

        Establish establish = () =>
                              {
                                  IncodingHtmlHelper.BootstrapVersion = BootstrapOfVersion.v3;
                                  IncodingHtmlHelper.Def_Control_Class = B.Col_xs_11;
                                  IncodingHtmlHelper.Def_Label_Class = B.Col_xs_1;

                                  Expression<Func<FakeModel, object>> expression = model => model.Prop;
                                  var label = new IncLabelControl(mockHtmlHelper.Original, expression);
                                  var input = new IncCheckBoxControl<FakeModel, object>(mockHtmlHelper.Original, expression);
                                  var validation = new IncValidationControl(mockHtmlHelper.Original, expression);
                                  control = new IncHorizontalControl<IncCheckBoxControl<FakeModel, object>>(label, input, validation);
                              };

        Because of = () => { result = control.ToHtmlString(); };

        It should_be_render = () => result.ToString()
                                          .ShouldEqual(@"<div class=""form-group""><label class=""control-label col-xs-1"" for=""Prop"">Prop</label><div class=""col-xs-11""><div class="" checkbox""><label><input id=""Prop"" name=""Prop"" type=""checkbox"" value=""true"" /><input name=""Prop"" type=""hidden"" value=""false"" /><i></i><span></span></label></div></div></div>");
    }
}