namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System;
    using System.Collections.Specialized;
    using Incoding.CQRS;
    using Incoding.MSpecContrib;
    using Incoding.MvcContrib.MVD;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(DispatcherControllerBase))]
    public class When_base_dispatcher_controller_push : Context_dispatcher_controller
    {
        static FakeByNameCommand command;

        Establish establish = () =>
                              {
                                  command = Pleasure.Generator.Invent<FakeByNameCommand>();
                                  Establish(types: new[] { command.GetType() });
                                  requestBase.SetupGet(r => r.Form).Returns(new NameValueCollection()
                                                                            {
                                                                                    { "Name", command.Name },
                                                                            });
                              };

        Because of = () => { result = controller.Push(typeof(FakeByNameCommand).Name); };

        It should_be_push = () => dispatcher.ShouldBePush(command);

        It should_be_result = () => result.ShouldBeIncodingSuccess();

        #region Fake classes

        public class FakeByNameCommand : CommandBase
        {
            public string Name { get; set; }

            ////ncrunch: no coverage start
            protected override void Execute()
            {
                throw new NotImplementedException();
            }
        }

        #endregion
    }
}