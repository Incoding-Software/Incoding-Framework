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
    public class When_base_dispatcher_controller_push_inner_class_by_name : Context_dispatcher_controller
    {
        #region Fake classes

        public class FakeInnerByNameCommand
        {
            #region Nested classes

            public class InnerByName : CommandBase
            {
                protected override void Execute()
                {
                    throw new NotImplementedException();
                }
            }

            #endregion
        }

        #endregion

        Establish establish = () => Establish(types: new[] { typeof(FakeInnerByNameCommand.InnerByName) });

        Because of = () => { result = controller.Push(HttpUtility.UrlEncode(typeof(FakeInnerByNameCommand.InnerByName).Name)); };

        It should_be_push = () => dispatcher.ShouldBePush(new FakeInnerByNameCommand.InnerByName());

        It should_be_result = () => result.ShouldBeIncodingSuccess();
    }
}