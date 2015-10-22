namespace Incoding.UnitTest
{
    #region << Using >>

    using Incoding.CQRS;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(DefaultDispatcher))]
    public class When_default_dispatcher_push_performance : Context_default_dispatcher
    {
        #region Establish value

        static CommandWithRepository message;

        #endregion

        Establish establish = () => { message = Pleasure.Generator.Invent<CommandWithRepository>(); };

        It should_be_performance = () => Pleasure.Do(i => dispatcher.Push(message), 1000).ShouldBeLessThan(700);
    }
}