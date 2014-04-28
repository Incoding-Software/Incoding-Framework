namespace Incoding.UnitTest
{
    using System.Diagnostics;
    using Incoding.CQRS;
    using Incoding.MSpecContrib;
    using Machine.Specifications;
    using Moq;
    using It = Machine.Specifications.It;

    [Subject(typeof(DefaultDispatcher))]
    public class When_default_dispatcher_push_performance : Context_default_dispatcher
    {
        #region Establish value

        static Mock<CommandBase> message;

        #endregion

        Establish establish = () => { message = Pleasure.Mock<CommandBase>(); };

        It should_be_performance = () => Pleasure.Do(i => dispatcher.Push(message.Object), 1000)
                                                 .ShouldBeLessThan(700);
    }
}