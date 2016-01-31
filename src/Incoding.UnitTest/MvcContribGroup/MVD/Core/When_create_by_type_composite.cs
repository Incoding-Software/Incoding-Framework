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

    [Subject(typeof(CreateByTypeQuery))]
    public class When_create_by_type_composite: Context_dispatcher_controller
    {
        #region Fake classes

        public class FakePushComposite1ByNameCommand : CommandBase
        {
            ////ncrunch: no coverage start
            protected override void Execute()
            {
                throw new NotImplementedException();
            }

            ////ncrunch: no coverage end       
        }

        public class FakePushComposite2ByNameCommand : CommandBase
        {
            ////ncrunch: no coverage start
            protected override void Execute()
            {
                throw new NotImplementedException();
            }

            ////ncrunch: no coverage end       
        }

        #endregion

        
        Because of = () =>
                     {
                         result = controller.Push("{0}&{1}".F(typeof(FakePushComposite1ByNameCommand).Name,
                                                                   typeof(FakePushComposite2ByNameCommand).Name));
                     };

        It should_be_push_1 = () => dispatcher.ShouldBePush(new FakePushComposite1ByNameCommand());

        It should_be_push_2 = () => dispatcher.ShouldBePush(new FakePushComposite2ByNameCommand());
    }
}