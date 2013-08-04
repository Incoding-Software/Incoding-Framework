namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System.Web.Mvc;
    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(IncHiddenControl<,>))]
    public class When_inc_help_block_control_without_message : Context_inc_control
    {
        #region Estabilish value

        static IncHelpBlockControl control;

        static string result;

        #endregion

        Establish establish = () => { control = new IncHelpBlockControl(); };

        Because of = () => { result = control.Render().ToHtmlString(); };

        It should_be_render = () => result.ShouldEqual(MvcHtmlString.Empty.ToHtmlString());
    }
}