namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System;
    using System.Collections.Specialized;
    using System.Web;
    using Incoding.CQRS;
    using Incoding.MSpecContrib;
    using Incoding.MvcContrib.MVD;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(DispatcherControllerBase))]
    public class When_base_dispatcher_controller_push_with_encode : Context_dispatcher_controller
    {
        static FakeByNameWithEncodeCommand command;

        Establish establish = () =>
                              {
                                  command = Pleasure.Generator.Invent<FakeByNameWithEncodeCommand>();
                                  Establish(types: new[] { command.GetType() });
                                  requestBase.SetupGet(r => r.Form).Returns(new NameValueCollection()
                                                                            {
                                                                                    { "Name", command.Name },
                                                                            });
                              };

        Because of = () => { result = controller.Push(HttpUtility.UrlEncode(typeof(FakeByNameWithEncodeCommand).Name)); };

        It should_be_push = () => dispatcher.ShouldBePush(command);

        #region Fake classes

        public class FakeByNameWithEncodeCommand : CommandBase
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