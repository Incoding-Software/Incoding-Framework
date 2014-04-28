namespace Incoding.UnitTest
{
    using System.Data;
    using Incoding.CQRS;
    using Incoding.MSpecContrib;
    using Machine.Specifications;
    using Moq;
    using It = Machine.Specifications.It;

    [Subject(typeof(DefaultDispatcher))]
    public class When_default_dispatcher_push_with_setting_closure : Context_default_dispatcher
    {
        #region Fake classes

        class FakeMessage : CommandBase
        {
            public override void Execute() { }
        }

        #endregion

        #region Establish value

        static FakeMessage message;

        #endregion

        Establish establish = () =>
                                  {
                                      unitOfWork.Setup(r => r.IsOpen()).Returns(true);
                                      message = new FakeMessage();
                                  };

        Because of = () =>
                         {
                             var setting = new MessageExecuteSetting { };
                             dispatcher.Push(message, setting);
                             dispatcher.Push(message, setting);
                         };

        It should_be_factory_create = () => unitOfWorkFactory.Verify(r => r.Create(IsolationLevel.ReadCommitted, Pleasure.MockIt.IsNull<string>()), Times.Exactly(2));
    }
}