namespace Incoding.UnitTest
{
    #region << Using >>

    using System.Data;
    using Incoding.CQRS;
    using Incoding.MSpecContrib;
    using Machine.Specifications;
    using Moq;
    using It = Machine.Specifications.It;

    #endregion

    [Subject(typeof(DefaultDispatcher))]
    public class When_default_dispatcher_push_with_setting_closure : Context_default_dispatcher
    {
        #region Establish value

        static FakeMessage message;

        #endregion

        Establish establish = () => { message = new FakeMessage(); };

        Because of = () =>
                     {
                         var setting = new MessageExecuteSetting { };
                         dispatcher.Push(message, setting);
                         dispatcher.Push(message, setting);
                     };

        It should_be_factory_create = () => unitOfWorkFactory.Verify(r => r.Create(IsolationLevel.ReadCommitted, Pleasure.MockIt.IsNull<string>()), Times.Exactly(2));

        #region Fake classes

        class FakeMessage : CommandBase
        {
            protected override void Execute() { }
        }

        #endregion
    }
}