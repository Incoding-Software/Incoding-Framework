namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System;
    using Incoding.CQRS;
    using Incoding.MSpecContrib;
    using Incoding.MvcContrib.MVD;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(DispatcherControllerBase))]
    public class When_base_dispatcher_controller_push_inner_class_without_encode : Context_dispatcher_controller
    {
        #region Fake classes

        public class FakeAddUserCommand
        {
            #region Nested classes

            public class Inner : CommandBase
            {
                public override void Execute()
                {
                    throw new NotImplementedException();
                }
            }

            #endregion
        }

        #endregion

        Establish establish = () => Establish(types: new[] { typeof(FakeAddUserCommand.Inner) });

        Because of = () => { result = controller.Push(typeof(FakeAddUserCommand.Inner).FullName, string.Empty); };

        It should_be_push = () => dispatcher.ShouldBePush(new FakeAddUserCommand.Inner());

        It should_be_result = () => result.ShouldBeIncodingSuccess();
    }
}