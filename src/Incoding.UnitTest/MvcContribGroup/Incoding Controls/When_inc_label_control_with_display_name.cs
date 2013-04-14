namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System;
    using System.Linq.Expressions;
    using Incoding.MvcContrib;
    using Machine.Specifications;using Incoding.MSpecContrib;

    #endregion

    [Subject(typeof(IncHiddenControl<,>))]
    public class When_inc_label_control_with_display_name : Context_inc_control
    {
        #region Estabilish value

        static IncLabelControl control;

        static string result;

        #endregion

        Establish establish = () =>
                                  {
                                      Expression<Func<FakeModel, string>> expression = model => model.DisplayName;
                                      control = new IncLabelControl(mockHtmlHelper.Original, expression);
                                  };

        Because of = () => { result = control.Render().ToHtmlString(); };

        It should_be_render = () => result.ShouldEqual("<label class=\"control-label\" for=\"DisplayName\">NameDisplay</label>");
    }
}