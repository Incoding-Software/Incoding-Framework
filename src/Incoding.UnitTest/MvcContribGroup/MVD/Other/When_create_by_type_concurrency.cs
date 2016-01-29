namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System;
    using System.Web;
    using Incoding.CQRS;
    using Incoding.MSpecContrib;
    using Incoding.MvcContrib.MVD;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(CreateByTypeQuery))]
    public class When_create_by_type_concurrency : Context_dispatcher_controller
    {
        #region Fake classes

        public class ConcurrentCommand : CommandBase
        {
            ////ncrunch: no coverage start
            protected override void Execute()
            {
                throw new NotImplementedException();
            }

            ////ncrunch: no coverage end       
        }

        #endregion

        #region Establish value

        static Exception exception;

        #endregion

        Establish establish = () => {};

        Because of = () => { exception = Catch.Exception(() => Pleasure.MultiThread.Do(() => controller.Push(HttpUtility.UrlEncode(typeof(ConcurrentCommand).Name)), 100)); };

        It should_be_without_exception = () => exception.ShouldBeNull();
    }
}