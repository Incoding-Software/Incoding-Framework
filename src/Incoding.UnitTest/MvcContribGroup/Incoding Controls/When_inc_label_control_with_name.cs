namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System;
    using System.Linq.Expressions;
    using Incoding.MSpecContrib;
    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(IncHiddenControl<,>))]
    public class When_inc_label_control_with_name : Context_inc_control
    {
        #region Establish value

        static IncLabelControl control;

        #endregion

        Establish establish = () =>
                                  {
                                      Expression<Func<FakeModel, string>> expression = model => model.Prop;
                                      control = new IncLabelControl(mockHtmlHelper.Original, expression);
                                      control.Name = Pleasure.Generator.TheSameString();
                                  };

        Because of = () => { result = control.ToHtmlString(); };

        It should_be_render = () => result.ToString()
                                          .ShouldEqual("<label for=\"Prop\">TheSameString</label>");
    }
}