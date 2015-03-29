namespace Incoding.UnitTest.MvcContribGroup
{
    using System;
    using Incoding.MvcContrib.MVD;
    using Machine.Specifications;

    [Subject(typeof(DispatcherControllerBase))]
    public class When_base_dispatcher_controller_push_composite_with_empty_inc_types : Context_dispatcher_controller
    {
        #region Establish value

        static Exception exception;

        #endregion

        Because of = () => { exception = Catch.Exception(() => controller.Composite(string.Empty)); };

        It should_be_argument_exception = () => exception.ShouldNotBeOfType(typeof(ArgumentException));
    }
}