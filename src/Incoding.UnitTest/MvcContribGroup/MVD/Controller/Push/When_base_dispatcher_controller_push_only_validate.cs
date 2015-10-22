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
    public class When_base_dispatcher_controller_push_only_validate : Context_dispatcher_controller
    {
        #region Fake classes

        public class FakeOnlyValidateCommand : CommandBase
        {
            #region Properties

            public string Name { get; set; }

            #endregion

            ////ncrunch: no coverage start
            protected override void Execute()
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region Establish value

        static FakeOnlyValidateCommand command;

        #endregion

        Establish establish = () =>
                              {
                                  command = Pleasure.Generator.Invent<FakeOnlyValidateCommand>();
                                  Establish(types: new[] { command.GetType() });
                              };

        Because of = () => { result = controller.Push(typeof(FakeOnlyValidateCommand).Name, incOnlyValidate: true); };

        It should_be_push = () => dispatcher.ShouldBePush(command, callCount: 0);

        It should_be_result = () => result.ShouldBeIncodingSuccess();
    }
}