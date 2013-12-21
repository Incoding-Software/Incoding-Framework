namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Web;
    using Incoding.CQRS;
    using Incoding.Extensions;
    using Incoding.MSpecContrib;
    using Incoding.MvcContrib.MVD;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(DispatcherControllerBase))]
    public class When_base_dispatcher_controller_push_composite_by_full_name : Context_dispatcher_controller
    {
        #region Fake classes

        public class FakePushComposite1ByFullNameCommand : CommandBase
        {
            #region Constructors

            public FakePushComposite1ByFullNameCommand()
            {
                Result = 1;
            }

            #endregion

            ////ncrunch: no coverage start
            public override void Execute()
            {
                throw new NotImplementedException();
            }

            ////ncrunch: no coverage end       
        }

        public class FakePushComposite2ByFullNameCommand : CommandBase
        {
            #region Constructors

            public FakePushComposite2ByFullNameCommand()
            {
                Result = 2;
            }

            #endregion

            ////ncrunch: no coverage start
            public override void Execute()
            {
                throw new NotImplementedException();
            }

            ////ncrunch: no coverage end       
        }

        #endregion

        Establish establish = () => Establish(types: new[] { typeof(FakePushComposite1ByFullNameCommand), typeof(FakePushComposite2ByFullNameCommand) });

        Because of = () =>
                         {
                             result = controller.Composite("{0},{1}".F(HttpUtility.UrlEncode(typeof(FakePushComposite1ByFullNameCommand).FullName), 
                                                                       HttpUtility.UrlEncode(typeof(FakePushComposite2ByFullNameCommand).FullName)));
                         };

        It should_be_push_1 = () => dispatcher.ShouldBePush(new FakePushComposite1ByFullNameCommand());

        It should_be_push_2 = () => dispatcher.ShouldBePush(new FakePushComposite2ByFullNameCommand());

        It should_be_result = () => result.ShouldBeIncodingSuccess<IEnumerable<object>>(list => list.ShouldEqualWeak(new[] { 1, 2 }));
    }
}