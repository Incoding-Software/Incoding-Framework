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
    public class When_create_by_type_with_inner_class_by_full_name : Context_dispatcher_controller
    {
        Because of = () => { result = controller.Push(HttpUtility.UrlEncode(typeof(FakeInnerByFullNameCommand.InnerByFullName).FullName)); };

        It should_be_push = () => dispatcher.ShouldBePush(new FakeInnerByFullNameCommand.InnerByFullName());

        It should_be_result = () => result.ShouldBeIncodingSuccess();

        #region Fake classes

        public class FakeInnerByFullNameCommand
        {
            #region Nested classes

            public class InnerByFullName : CommandBase
            {
                protected override void Execute()
                {
                    throw new NotImplementedException();
                }
            }

            #endregion
        }

        #endregion
    }
}