namespace Incoding.UnitTest
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Linq;
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
                                  var sameSetting = Pleasure.Generator.Invent<MessageExecuteSetting>(dsl => { });

                                  fakeCommands = Pleasure.ToList(Pleasure.Generator.Invent<FakeCommand>(factoryDsl => factoryDsl.GenerateTo(r => r.Setting, dsl => { })),
                                                                 Pleasure.Generator.Invent<FakeCommand>(factoryDsl => factoryDsl.Tuning(r => r.Setting, sameSetting)),
                                                                 Pleasure.Generator.Invent<FakeCommand>(factoryDsl => factoryDsl.Tuning(r => r.Setting, sameSetting)))
                                                         .Cast<IMessage<object>>()
                                                         .ToList();
                                  nextDt = null;//Pleasure.Generator.DateTime();
                                  var command = Pleasure.Generator.Invent<AddDelayToSchedulerCommand>(dsl => dsl.Tuning(r => r.Commands, fakeCommands)
                                                                                                                .Tuning(r => r.Recurrency, Pleasure.Generator.Invent<GetRecurrencyDateQuery>()));

                                  mockCommand = MockCommand<AddDelayToSchedulerCommand>
                                          .When(command)
                                          .StubQuery(command.Recurrency, nextDt);
                              };

        Because of = () => mockCommand.Original.Execute();

        It should_be_saves = () => mockCommand.ShouldBeSaves<DelayToScheduler>(scheduler => scheduler.ShouldEqualWeakEach(fakeCommands,
                                                                                                                          (dsl, i) => dsl.IgnoreBecauseCalculate(r => r.Id)
                                                                                                                                         .ForwardToValue(r => r.UID, mockCommand.Original.UID)
                                                                                                                                         .ForwardToValue(r => r.Type, typeof(FakeCommand).AssemblyQualifiedName)
                                                                                                                                         .ForwardToValue(r => r.Priority, i)
                                                                                                                                         .ForwardToValue(r => r.StartsOn, nextDt.GetValueOrDefault(mockCommand.Original.Recurrency.StartDate.GetValueOrDefault(DateTime.UtcNow)))
                                                                                                                                         .ForwardToValue(r => r.Status, DelayOfStatus.New)
                                                                                                                                         .ForwardToValue(r => r.Command, fakeCommands[i].ToJsonString())
                                                                                                                                         .ForwardToValue(r => r.Recurrence, mockCommand.Original.Recurrency)
                                                                                                                                         .IgnoreBecauseNotUse(r => r.Description)
                                                                                                                                         .IgnoreBecauseCalculate(r => r.Instance)
                                                                                                                                         .ForwardToAction(r => r.GroupKey, toScheduler => toScheduler.GroupKey.ShouldNotBeEmpty())));

        #region Fake classes

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

        static List<IMessage<object>> fakeCommands;

        static DateTime? nextDt;

        #endregion
    }
}