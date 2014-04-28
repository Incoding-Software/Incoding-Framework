namespace Incoding.UnitTest
{
    #region << Using >>

    using System;
    using Incoding.CQRS;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(DefaultDispatcher))]
    public class When_default_dispatcher_push_throw_with_mute_event : Behavior_default_dispatcher_push_with_mute_event
    {
        Establish establish = () => eventBroker.Setup(r => r.HasSubscriber(Pleasure.MockIt.IsAny<OnAfterErrorExecuteEvent>())).Returns(true);

        Because of = () =>
                         {
                             exception = Catch.Exception(() => dispatcher.Push(Pleasure.MockAsObject<CommandBase>(mock => mock.Setup(r => r.Execute()).Throws<ArgumentException>()), new MessageExecuteSetting
                                                                                                                                                                                         {
                                                                                                                                                                                                 Mute = MuteEvent.OnAfter |
                                                                                                                                                                                                        MuteEvent.OnBefore |
                                                                                                                                                                                                        MuteEvent.OnComplete |
                                                                                                                                                                                                        MuteEvent.OnError,
                                                                                                                                                                                         }));
                         };

        Behaves_like<Behavior_default_dispatcher_push_with_mute_event> should_be_verify;
    }
}