namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System;
    using System.Linq.Expressions;
    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(IncHiddenControl<,>))]
    public class When_inc_label_control_with_property_name : Context_inc_control
    {
        #region Establish value

        static IncLabelControl control;

        #endregion

        Establish establish = () =>
                                  {
                                      Expression<Func<FakeModel, string>> expression = model => model.Prop;
                                      control = new IncLabelControl(mockHtmlHelper.Original, expression);
                                  };

        Because of = () => { result = control.ToHtmlString(); };

        It should_be_render = () => result.ToString()
                                          .ShouldEqual("<label for=\"Prop\">Prop</label>");
    }
}