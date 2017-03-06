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
        Establish establish = () =>
                              {
                                  When_add_delay_to_scheduler.command = Pleasure.Generator.Invent<FakeCommand>(factoryDsl => factoryDsl.GenerateTo(r => r.Setting, dsl => { }));
                                  nextDt = Pleasure.Generator.DateTime();
                                  var command = Pleasure.Generator.Invent<AddDelayToSchedulerCommand>(dsl => dsl.Tuning(r => r.Command, When_add_delay_to_scheduler.command)
                                                                                                                .Tuning(r => r.Recurrency, Pleasure.Generator.Invent<GetRecurrencyDateQuery>()));

                                  mockCommand = MockCommand<AddDelayToSchedulerCommand>
                                          .When(command)
                                          .StubQuery(command.Recurrency, nextDt);
                              };

        Because of = () => mockCommand.Original.Execute();

        It should_be_saves = () => mockCommand.ShouldBeSave<DelayToScheduler>(scheduler => scheduler.ShouldEqualWeak(command, (dsl) => dsl.IgnoreBecauseCalculate(r => r.Id)
                                                                                                                                          .ForwardToValue(r => r.Option, new DelayToScheduler.OptionOfDelay()
                                                                                                                                                                         {
                                                                                                                                                                                 Async = true
                                                                                                                                                                         })
                                                                                                                                          .ForwardToAction(r => r.CreateDt, toScheduler => toScheduler.CreateDt.ShouldBeDate(DateTime.UtcNow))
                                                                                                                                          .ForwardToValue(r => r.UID, mockCommand.Original.UID)
                                                                                                                                          .ForwardToValue(r => r.Type, typeof(FakeCommand).AssemblyQualifiedName)
                                                                                                                                          .ForwardToValue(r => r.StartsOn, mockCommand.Original.Recurrency.StartDate.Value)
                                                                                                                                          .ForwardToValue(r => r.Status, DelayOfStatus.New)
                                                                                                                                          .ForwardToValue(r => r.Priority, mockCommand.Original.Priority)
                                                                                                                                          .ForwardToValue(r => r.Command, command.ToJsonString())
                                                                                                                                          .ForwardToValue(r => r.Recurrence, mockCommand.Original.Recurrency)
                                                                                                                                          .IgnoreBecauseNotUse(r => r.Description)));

        #region Fake classes

        [OptionOfDelay(Async = true)]
        public class FakeCommand : CommandBase
        {
            protected override void Execute() { }

            #region Properties

            public string Prop1 { get; set; }

            public string Prop2 { get; set; }

            public string Prop3 { get; set; }

            #endregion
        }

        #endregion

        #region Establish value

        static MockMessage<AddDelayToSchedulerCommand, object> mockCommand;

        static CommandBase command;

        static DateTime? nextDt;

        #endregion
    }
}