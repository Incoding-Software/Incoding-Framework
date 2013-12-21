namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System;
    using System.Linq.Expressions;
    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(IncHiddenControl<,>))]
    public class When_inc_label_control_with_display_name : Context_inc_control
    {
        #region Estabilish value

        static IncLabelControl control;

        #endregion

        Establish establish = () =>
                                  {
                                      Expression<Func<FakeModel, string>> expression = model => model.DisplayName;
                                      control = new IncLabelControl(mockHtmlHelper.Original, expression);
                                  };

        Because of = () => { result = control.ToHtmlString(); };

        It should_be_render = () => result.ToString()
                                          .ShouldEqual("<label for=\"DisplayName\">NameDisplay</label>");
    }
}