namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System;
    using System.Linq.Expressions;
    using Incoding.MvcContrib;
    using Machine.Specifications;using Incoding.MSpecContrib;

    #endregion

    [Subject(typeof(IncHiddenControl<,>))]
    public class When_inc_horizontal_control : Context_inc_control
    {
        #region Estabilish value

        static IncHorizontalControl<IncHiddenControl<FakeModel, object>> control;

        static string result;

        #endregion

        Establish establish = () =>
                                  {
                                      Expression<Func<FakeModel, object>> expression = model => model.Prop;
                                      var label = new IncLabelControl(mockHtmlHelper.Original, expression);
                                      var input = new IncHiddenControl<FakeModel, object>(mockHtmlHelper.Original, expression);
                                      var validation = new IncValidationControl(mockHtmlHelper.Original, expression);
                                      control = new IncHorizontalControl<IncHiddenControl<FakeModel, object>>(label, input, validation);
                                  };

        Because of = () => { result = control.Render().ToHtmlString(); };

        It should_be_render = () => result.ShouldEqual("<div class=\"control-group\"><label class=\"control-label\" for=\"Prop\">Prop</label><div class=\"controls\"><input id=\"Prop\" name=\"Prop\" type=\"hidden\" value=\"TheSameString\" /></div></div>");
    }
}