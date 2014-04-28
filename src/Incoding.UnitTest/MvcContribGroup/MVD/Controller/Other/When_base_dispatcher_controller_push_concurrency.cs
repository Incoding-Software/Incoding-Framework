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

    [Subject(typeof(DispatcherControllerBase))]
    public class When_base_dispatcher_controller_push_concurrency : Context_dispatcher_controller
    {
        #region Fake classes

        public class ConcurrentCommand : CommandBase
        {
            ////ncrunch: no coverage start
            public override void Execute()
            {
                throw new NotImplementedException();
            }

            ////ncrunch: no coverage end       
        }

        #endregion

        #region Establish value

        static Exception exception;

        #endregion

        Establish establish = () => Establish(types: new[] { typeof(ConcurrentCommand) });

        Because of = () => { exception = Catch.Exception(() => Pleasure.MultiThread.Do(() => controller.Push(HttpUtility.UrlEncode(typeof(ConcurrentCommand).Name), string.Empty), 100)); };

        It should_be_without_exception = () => exception.ShouldBeNull();
    }
}