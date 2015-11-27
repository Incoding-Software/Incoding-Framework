namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System;
    using Incoding.MvcContrib.MVD;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(DispatcherControllerBase))]
    public class When_base_dispatcher_controller_push_with_empty_inc_types : Context_dispatcher_controller
    {
        #region Establish value

        static Exception exception;

        #endregion

        Establish establish = () => Establish();

        Because of = () => { exception = Catch.Exception(() => controller.Push(string.Empty)); };

        It should_be_argument_exception = () => exception.ShouldBeAssignableTo<ArgumentException>();
    }
}