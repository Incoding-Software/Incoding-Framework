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
    public class When_base_dispatcher_controller_push_performance : Context_dispatcher_controller
    {
        #region Fake classes

        public class PerformanceCommand : CommandBase
        {
            ////ncrunch: no coverage start
            protected override void Execute()
            {
                throw new NotImplementedException();
            }

            ////ncrunch: no coverage end       
        }

        #endregion

        Establish establish = () => Establish(types: new[] { typeof(PerformanceCommand) });

        It should_be_performance = () => Pleasure
                                                 .Do(i => controller.Push(HttpUtility.UrlEncode(typeof(PerformanceCommand).Name)), 1000)
                                                 .ShouldBeLessThan(600);
    }
}