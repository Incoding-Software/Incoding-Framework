namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System;
    using System.Web;
    using Incoding.MvcContrib.MVD;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(DispatcherControllerBase))]
    public class When_base_dispatcher_controller_wrong_generic_instance : Context_dispatcher_controller
    {
        #region Establish value

        static Exception exception;

        #endregion

        Establish establish = () => Establish(types: new[] { typeof(string) });

        Because of = () => { exception = Catch.Exception(() => controller.Push(HttpUtility.UrlEncode(typeof(string).FullName), "bad")); };

        It should_be_exception = () => exception.Message.ShouldEqual("Not found any generic type bad");
    }
}