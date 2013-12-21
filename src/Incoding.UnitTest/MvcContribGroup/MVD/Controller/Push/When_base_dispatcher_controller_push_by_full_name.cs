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
    public class When_base_dispatcher_controller_push_by_full_name : Context_dispatcher_controller
    {
        #region Fake classes

        public class FakeByFullNameCommand : CommandBase
        {
            #region Constructors

            public FakeByFullNameCommand()
            {
                Result = 5;
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

        Establish establish = () => Establish(types: new[] { typeof(FakeByFullNameCommand) });

        Because of = () => { result = controller.Push(HttpUtility.UrlEncode(typeof(FakeByFullNameCommand).FullName), string.Empty); };

        It should_be_push = () => dispatcher.ShouldBePush(new FakeByFullNameCommand());

        It should_be_result = () => result.ShouldBeIncodingSuccess<int>(i => i.ShouldEqual(5));
    }
}