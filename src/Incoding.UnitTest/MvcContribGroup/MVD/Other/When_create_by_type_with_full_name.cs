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
    public class When_create_by_type_with_full_name : Context_dispatcher_controller
    {
        Because of = () => { result = controller.Push(HttpUtility.UrlEncode(typeof(FakeByFullNameCommand).FullName)); };

        It should_be_push = () => dispatcher.ShouldBePush(new FakeByFullNameCommand());

        It should_be_result = () => result.ShouldBeIncodingSuccess<int>(i => i.ShouldEqual(5));

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
            protected override void Execute()
            {
                throw new NotImplementedException();
            }

            ////ncrunch: no coverage end       
        }

        #endregion
    }
}