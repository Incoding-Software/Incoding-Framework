namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using Incoding.MSpecContrib;
    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(IncHiddenControl<,>))]
    public class When_inc_help_block_control : Context_inc_control
    {
        #region Establish value

        static IncHelpBlockControl control;

        #endregion

        Establish establish = () =>
                                  {
                                      control = new IncHelpBlockControl();
                                      control.Message = Pleasure.Generator.TheSameString();
                                      control.AddClass(B.Col_xs_12);
                                  };

        Because of = () => { result = control.ToHtmlString(); };

        It should_be_render = () => result.ToString()
                                          .ShouldEqual("<p class=\"help-block col-xs-12\">TheSameString</p>");
    }
}