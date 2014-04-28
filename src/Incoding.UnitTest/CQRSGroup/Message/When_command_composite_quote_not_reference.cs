namespace Incoding.UnitTest
{
    using System;
    using Incoding.CQRS;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    [Subject(typeof(CommandComposite))]
    public class When_command_composite_quote_not_reference
    {
        #region Fake classes

        class FakeCommand : CommandBase
        {
            public override void Execute()
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region Establish value

        static MessageExecuteSetting setting;

        static CommandComposite composite;

        #endregion

        Establish establish = () => { setting = Pleasure.Generator.Invent<MessageExecuteSetting>(dsl => dsl.GenerateTo(r => r.Delay)); };

        Because of = () => { composite = (CommandComposite)new CommandComposite().Quote(Pleasure.Generator.Invent<FakeCommand>(), setting); };

        It should_be_equal = () => composite.Parts[0].ShouldEqualWeak(setting);

        It should_be_not_the_same = () => composite.Parts[0].ShouldNotBeTheSameAs(setting);
    }
}