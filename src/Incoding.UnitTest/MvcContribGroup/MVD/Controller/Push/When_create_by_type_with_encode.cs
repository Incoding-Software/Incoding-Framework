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
    public class When_create_by_type_with_encode : Context_dispatcher_controller
    {
        static FakeByNameWithEncodeCommand command;

        Establish establish = () => { command = Pleasure.Generator.Invent<FakeByNameWithEncodeCommand>(); };

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