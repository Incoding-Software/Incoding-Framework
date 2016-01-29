namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System;
    using Incoding.MvcContrib.MVD;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(DispatcherControllerBase))]
    public class When_create_by_type_with_wrong_instance : Context_dispatcher_controller
    {
        #region Establish value

        static Exception exception;

        #endregion

        
        Because of = () => { exception = Catch.Exception(() => controller.Push("bad")); };

        It should_be_exception = () => exception.Message.ShouldEqual("Not found any type bad");
    }
}