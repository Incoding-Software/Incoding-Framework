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
        #region Establish value

        static IncHelpBlockControl control;

        #endregion

        Establish establish = () => { control = new IncHelpBlockControl(); };

        Because of = () => { result = control.ToHtmlString(); };

        It should_be_render = () => result.ToString()
                                          .ShouldEqual(MvcHtmlString.Empty.ToHtmlString());
    }
}