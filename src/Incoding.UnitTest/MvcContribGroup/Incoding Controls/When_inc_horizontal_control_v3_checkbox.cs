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
                                  IncodingHtmlHelper.Def_Control_Class = B.Col_xs_7;
                                  IncodingHtmlHelper.Def_Label_Class = B.Col_xs_5;

                                  Expression<Func<FakeModel, object>> expression = model => model.Prop;
                                  var label = new IncLabelControl(mockHtmlHelper.Original, expression);
                                  var input = new IncCheckBoxControl<FakeModel, object>(mockHtmlHelper.Original, expression);
                                  var validation = new IncValidationControl(mockHtmlHelper.Original, expression);
                                  control = new IncHorizontalControl<IncCheckBoxControl<FakeModel, object>>(label, input, validation);
                                  
                              };

        Because of = () => { result = control.ToHtmlString(); };

        It should_be_render = () => result.ToString()
                                          .ShouldEqual(@"<div class=""form-group""><label class=""control-label col-xs-5"" for=""Prop"">Prop</label><div class=""col-xs-7""><div class="" checkbox""><label><input id=""Prop"" name=""Prop"" type=""checkbox"" value=""true"" /><input name=""Prop"" type=""hidden"" value=""false"" /><i></i><span></span></label></div></div></div>");

        Cleanup clean = () => IncodingHtmlHelper.BootstrapVersion = BootstrapOfVersion.v2;
    }
}
