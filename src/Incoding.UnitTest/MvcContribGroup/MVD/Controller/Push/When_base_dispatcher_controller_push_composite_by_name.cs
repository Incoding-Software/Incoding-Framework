namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System;
    using System.Web;
    using Incoding.CQRS;
    using Incoding.Extensions;
    using Incoding.MSpecContrib;
    using Incoding.MvcContrib.MVD;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(DispatcherControllerBase))]
    public class When_base_dispatcher_controller_push_composite_by_name : Context_dispatcher_controller
    {
        #region Fake classes

        public class FakePushComposite1ByNameCommand : CommandBase
        {
            ////ncrunch: no coverage start
            public override void Execute()
            {
                throw new NotImplementedException();
            }

            ////ncrunch: no coverage end       
        }

        public class FakePushComposite2ByNameCommand : CommandBase
        {
            ////ncrunch: no coverage start
            public override void Execute()
            {
                throw new NotImplementedException();
            }

            ////ncrunch: no coverage end       
        }

        #endregion

        Establish establish = () => Establish(types: new[] { typeof(FakePushComposite1ByNameCommand), typeof(FakePushComposite2ByNameCommand) });

        Because of = () =>
                         {
                             result = controller.Composite("{0},{1}".F(HttpUtility.UrlEncode(typeof(FakePushComposite1ByNameCommand).Name),
                                                                       HttpUtility.UrlEncode(typeof(FakePushComposite2ByNameCommand).Name)));
                         };

        It should_be_push_1 = () => dispatcher.ShouldBePush(new FakePushComposite1ByNameCommand());

        It should_be_push_2 = () => dispatcher.ShouldBePush(new FakePushComposite2ByNameCommand());
    }
}