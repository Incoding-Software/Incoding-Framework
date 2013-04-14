namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using Incoding.MvcContrib;
    using Machine.Specifications;using Incoding.MSpecContrib;

    #endregion

    [Subject(typeof(IncHiddenControl<,>))]
    public class When_inc_help_block_control : Context_inc_control
    {
        #region Estabilish value

        static IncHelpBlockControl control;

        static string result;

        #endregion

        Establish establish = () =>
                                  {
                                      control = new IncHelpBlockControl();
                                      control.Message = Pleasure.Generator.TheSameString();
                                  };

        Because of = () => { result = control.Render().ToHtmlString(); };

        It should_be_render = () => result.ShouldEqual("<p class=\"help-block\">TheSameString</p>");
    }
}