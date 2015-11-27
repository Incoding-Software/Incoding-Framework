namespace Incoding.UnitTest
{
    #region << Using >>

    using System;
    using Incoding.CQRS;
    using Incoding.Data;
    using Incoding.MSpecContrib;
    using Machine.Specifications;
    using Moq;
    using It = Machine.Specifications.It;

    #endregion

    [Subject(typeof(DefaultDispatcher))]
    public class When_command_composite_quote_with_setting_exception : Context_default_dispatcher
    {
        #region Establish value

        static CommandComposite composite;

        static Mock<ISpy> onSpy;

        static Exception exception;

        #endregion

        Establish establish = () =>
                              {
                                  var message = Pleasure.Mock<CommandBase>(mock =>
                                                                           {
                                                                               mock.Setup(r => r.OnExecute(dispatcher, Pleasure.MockIt.IsNotNull<Lazy<IUnitOfWork>>())).Throws(new IncFakeException());
                                                                               mock.SetupGet(r=>r.Setting).ReturnsInvent();
                                                                           });

                                  onSpy = Pleasure.Spy();

                                  composite = new CommandComposite();
                                  composite.Quote(message.Object);
                              };

        Because of = () => { exception = Catch.Exception(() => dispatcher.Push(composite)); };

        It should_be_throw = () => exception.ShouldBeAssignableTo<IncFakeException>();
    }
}