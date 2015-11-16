namespace Incoding.UnitTest
{
    #region << Using >>

    using System;
    using Incoding.Block;
    using Incoding.CQRS;
    using Incoding.Extensions;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(AddDelayToSchedulerCommand))]
    public class When_add_delay_to_scheduler
    {
        #region Fake classes

        public class FakeCommand : CommandBase
        {
            #region Properties

            public string Prop1 { get; set; }

            public string Prop2 { get; set; }

            public string Prop3 { get; set; }

            #endregion

            protected override void Execute() { }
        }

        #endregion

        #region Establish value

        static MockMessage<AddDelayToSchedulerCommand, object> mockCommand;

        static CommandBase command;

        static DateTime? nextDt;

        #endregion

        Establish establish = () =>
                              {
                                  When_add_delay_to_scheduler.command = Pleasure.Generator.Invent<FakeCommand>(factoryDsl => factoryDsl.GenerateTo(r => r.Setting, dsl => { }));
                                  nextDt = null; // Pleasure.Generator.DateTime();
                                  var command = Pleasure.Generator.Invent<AddDelayToSchedulerCommand>(dsl => dsl.Tuning(r => r.Command, When_add_delay_to_scheduler.command)
                                                                                                                .Tuning(r => r.Recurrency, Pleasure.Generator.Invent<GetRecurrencyDateQuery>()));

                                  mockCommand = MockCommand<AddDelayToSchedulerCommand>
                                          .When(command)
                                          .StubQuery(command.Recurrency, nextDt);
                              };

        Because of = () => mockCommand.Original.Execute();

        It should_be_saves = () => mockCommand.ShouldBeSave<DelayToScheduler>(scheduler => scheduler.ShouldEqualWeak(command, (dsl) => dsl.IgnoreBecauseCalculate(r => r.Id)
                                                                                                                                          .ForwardToValue(r => r.UID, mockCommand.Original.UID)
                                                                                                                                          .ForwardToValue(r => r.Type, typeof(FakeCommand).AssemblyQualifiedName)
                                                                                                                                          .ForwardToValue(r => r.StartsOn, nextDt.GetValueOrDefault(mockCommand.Original.Recurrency.StartDate.GetValueOrDefault(DateTime.UtcNow)))
                                                                                                                                          .ForwardToValue(r => r.Status, DelayOfStatus.New)
                                                                                                                                          .ForwardToValue(r=>r.Priority,mockCommand.Original.Priority)
                                                                                                                                          .ForwardToValue(r => r.Command, command.ToJsonString())
                                                                                                                                          .ForwardToValue(r => r.Recurrence, mockCommand.Original.Recurrency)
                                                                                                                                          .IgnoreBecauseNotUse(r => r.Description)
                                                                                                                                          .IgnoreBecauseCalculate(r => r.Instance)));
    }
}