namespace Incoding.UnitTest
{
    #region << Using >>

    using Incoding.Block;
    using Incoding.CQRS;
    using Incoding.Extensions;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(DelayToScheduler))]
    public class When_delay_to_sceduler_get_instance
    {
        #region Fake classes

        class FakeCommand : CommandBase
        {
            // ReSharper disable UnusedMember.Local

            #region Properties

            public string Value { get; set; }

            public string Value2 { get; set; }

            #endregion

            // ReSharper restore UnusedMember.Local

            public override void Execute() { }
        }

        #endregion

        #region Establish value

        static DelayToScheduler delay;

        static CommandBase expected;

        static FakeCommand original;

        #endregion

        Establish establish = () =>
                                  {
                                      original = Pleasure.Generator.Invent<FakeCommand>(dsl => dsl.GenerateTo(r => r.Setting, factoryDsl => {}));
                                      delay = new DelayToScheduler
                                                  {
                                                          Command = original.ToJsonString(),
                                                          Type = typeof(FakeCommand).AssemblyQualifiedName
                                                  };
                                  };

        Because of = () => { expected = delay.Instance; };

        It should_be_equal = () => original.ShouldEqualWeak(expected);
    }
}