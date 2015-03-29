namespace Incoding.UnitTest
{
    #region << Using >>

    using System;
    using Incoding.CQRS;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(CommandComposite))]
    public class When_command_composite_fluent_api
    {
        #region Fake classes

        [MessageExecuteSetting(Connection = "TheSameConnectionString", DataBaseInstance = "TheSameDataBase", Mute = MuteEvent.OnAfter)]
        class FakeCommandWithAttribute : CommandBase
        {
            public override void Execute()
            {
                throw new NotImplementedException();
            }
        }

        class FakeCommand : CommandBase
        {
            public override void Execute()
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region Establish value

        static CommandComposite composite;

        static MessageExecuteSetting quoteSetting2;

        static FakeCommand quote1;

        static FakeCommand quote2;

        static FakeCommandWithAttribute quote3;

        #endregion

        Establish establish = () =>
                                  {
                                      quoteSetting2 = Pleasure.Generator.Invent<MessageExecuteSetting>(dsl => {});

                                      quote1 = Pleasure.Generator.Invent<FakeCommand>();
                                      quote2 = Pleasure.Generator.Invent<FakeCommand>();
                                      quote3 = Pleasure.Generator.Invent<FakeCommandWithAttribute>();
                                  };

        Because of = () =>
                         {
                             composite = (CommandComposite)new CommandComposite()
                                                                   .Quote(quote1)
                                                                   .Quote(Pleasure.Generator.Invent<FakeCommand>())
                                                                   .WithConnectionString(quoteSetting2.Connection)
                                                                   .WithDateBaseString(quoteSetting2.DataBaseInstance)
                                                                   .OnAfter(message => { })
                                                                   .OnBefore(message => { })
                                                                   .OnComplete(message => { })
                                                                   .OnError((message, exception) => { })
                                                                   .Mute(quoteSetting2.Mute)
                                                                   .Quote(quote3);
                         };

        It should_be_count = () => composite.Parts.Count.ShouldEqual(3);

        It should_be_quote_1 = () => composite.Parts[0].ShouldEqualWeak(quote1, dsl => dsl.ForwardToValue(r => r.Setting, new MessageExecuteSetting()));

        It should_be_quote_2 = () => composite.Parts[1].ShouldEqualWeak(quote2, dsl => dsl.ForwardToValue(r => r.Setting, quoteSetting2));

        It should_be_quote_3 = () => composite.Parts[2].ShouldEqualWeak(quote3, dsl => dsl.ForwardToValue(r => r.Setting, new MessageExecuteSetting
                                                                                                                              {
                                                                                                                                      Connection = "TheSameConnectionString", 
                                                                                                                                      DataBaseInstance = "TheSameDataBase", 
                                                                                                                                      Mute = MuteEvent.OnAfter, 
                                                                                                                              }));
    }
}