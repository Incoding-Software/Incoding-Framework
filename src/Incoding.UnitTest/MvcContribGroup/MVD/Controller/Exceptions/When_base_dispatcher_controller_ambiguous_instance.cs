namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System;
    using Incoding.MvcContrib.MVD;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(DispatcherControllerBase))]
    public class When_base_dispatcher_controller_ambiguous_instance : Context_dispatcher_controller
    {
        #region Establish value

        static Exception exception;

        #endregion

        Establish establish = () => Establish(types: new[] { typeof(string) });

        Because of = () => { exception = Catch.Exception(() => controller.Push("AmbiguousType", string.Empty)); };

        It should_be_exception = () => exception.Message.ShouldEqual("Ambiguous type AmbiguousType");
    }
}