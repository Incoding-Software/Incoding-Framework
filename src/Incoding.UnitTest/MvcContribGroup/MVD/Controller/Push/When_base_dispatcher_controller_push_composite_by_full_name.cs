namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using Incoding.CQRS;
    using Incoding.Extensions;
    using Incoding.MSpecContrib;
    using Incoding.MvcContrib.MVD;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(DispatcherControllerBase))]
    public class When_base_dispatcher_controller_push_composite_by_full_name : Context_dispatcher_controller
    {
        Establish establish = () => Establish(types: new[] { typeof(FakePushComposite1ByFullNameCommand), typeof(FakePushComposite2ByFullNameCommand) });

        Because of = () =>
                     {
                         result = controller.Push("{0}&{1}".F(typeof(FakePushComposite1ByFullNameCommand).FullName,
                                                              typeof(FakePushComposite2ByFullNameCommand).FullName));
                     };

        It should_be_push_1 = () => dispatcher.ShouldBePush(new FakePushComposite1ByFullNameCommand());

        It should_be_push_2 = () => dispatcher.ShouldBePush(new FakePushComposite2ByFullNameCommand());

        It should_be_result = () => result.ShouldBeIncodingSuccess<IEnumerable<object>>(list => list.ShouldEqualWeak(new[] { 1, 2 }));

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
            protected override void Execute()
            {
                throw new NotImplementedException();
            }
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
            protected override void Execute()
            {
                throw new NotImplementedException();
            }
        }

        #endregion
    }
}