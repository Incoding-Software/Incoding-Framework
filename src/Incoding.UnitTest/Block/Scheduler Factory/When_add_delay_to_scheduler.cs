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
        #region Fake classes

        public class FakeCommand : CommandBase
        {
            #region Properties

            public string Prop1 { get; set; }

            public string Prop2 { get; set; }

            public string Prop3 { get; set; }

            #endregion

            public override void Execute() { }
        }

        #endregion

        #region Establish value

        static MockMessage<AddDelayToSchedulerCommand, object> mockCommand;

        static List<IMessage<object>> fakeCommands;

        #endregion

        Establish establish = () =>
                                  {
                                      var sameSetting = Pleasure.Generator.Invent<MessageExecuteSetting>(dsl => dsl.GenerateTo(r => r.Delay));

                                      fakeCommands = Pleasure.ToList(Pleasure.Generator.Invent<FakeCommand>(factoryDsl => factoryDsl.GenerateTo(r => r.Setting, dsl => dsl.GenerateTo(r => r.Delay))), 
                                                                     Pleasure.Generator.Invent<FakeCommand>(factoryDsl => factoryDsl.Tuning(r => r.Setting, sameSetting)), 
                                                                     Pleasure.Generator.Invent<FakeCommand>(factoryDsl => factoryDsl.Tuning(r => r.Setting, sameSetting)))
                                                             .Cast<IMessage<object>>()
                                                             .ToList();
                                      var command = Pleasure.Generator.Invent<AddDelayToSchedulerCommand>(dsl => dsl.Tuning(r => r.Commands, fakeCommands));

                                      mockCommand = MockCommand<AddDelayToSchedulerCommand>
                                              .When(command);
                                  };

        Because of = () => mockCommand.Original.Execute();

        It should_be_save = () =>
                                {
                                    Action<int, int> itSave = (index, priority) => mockCommand.ShouldBeSave<DelayToScheduler>(scheduler => scheduler.ShouldEqualWeak(new
                                                                                                                                                                         {
                                                                                                                                                                                 Command = fakeCommands[index].ToJsonString(), 
                                                                                                                                                                                 Type = typeof(FakeCommand).AssemblyQualifiedName, 
                                                                                                                                                                                 Status = DelayOfStatus.New, 
                                                                                                                                                                                 Priority = priority
                                                                                                                                                                         }, 
                                                                                                                                                                     dsl => dsl.IgnoreBecauseCalculate(r => r.Id)
                                                                                                                                                                               .ForwardToValue(r => r.UID, fakeCommands[index].Setting.Delay.UID)
                                                                                                                                                                               .IgnoreBecauseNotUse(r => r.Description)
                                                                                                                                                                               .IgnoreBecauseCalculate(r => r.Instance)
                                                                                                                                                                               .ForwardToAction(r => r.GroupKey, toScheduler => toScheduler.GroupKey.ShouldNotBeEmpty())));

                                    itSave(0, 0);
                                    itSave(1, 1);
                                    itSave(2, 2);
                                };
    }
}