namespace Incoding.UnitTest
{
    #region << Using >>

    using System;
    using System.Linq;
    using Incoding.Block;
    using Incoding.CQRS;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(StartSchedulerCommand))]
    public class When_start_scheduler_with_success
    {
        Establish establish = () =>
                              {
                                  instance1 = Pleasure.Generator.Invent<FakeCommand>(dsl => dsl.GenerateTo(r => r.Setting));
                                  instance2 = Pleasure.Generator.Invent<FakeCommand>(dsl => dsl.GenerateTo(r => r.Setting));

                                  var command = Pleasure.Generator.Invent<StartSchedulerCommand>(dsl => dsl.Tuning(r => r.Conditional, () => true)
                                                                                                           .Ignore(r => r.DelayToStart, "default")   
                                                                                                           .Tuning(r=>r.FetchSize,15)                                                                                                        
                                                                                                           .GenerateTo(r => r.Setting));

                                  mockMessage = MockCommand<StartSchedulerCommand>
                                          .When(command)
                                          .StubQuery(Pleasure.Generator.Invent<GetExpectedDelayToSchedulerQuery>(dsl => dsl.Tuning(r => r.Async, true)
                                                                                                                           .Tuning(r => r.IncludeInProgress, true)
                                                                                                                           .Tuning(r => r.FetchSize, command.FetchSize)),
                                                     dsl => dsl.ForwardToAction(r => r.Date, query => query.Date.ShouldBeDate(DateTime.UtcNow)),
                                                     new[] { Pleasure.Generator.Invent<GetExpectedDelayToSchedulerQuery.Response>(dsl => dsl.Tuning(s => s.Id, "1").Tuning(r => r.Instance, instance2)) }.ToList())
                                          .StubQuery(Pleasure.Generator.Invent<GetExpectedDelayToSchedulerQuery>(dsl => dsl.Tuning(r => r.Async, false)
                                                                                                                           .Tuning(r => r.IncludeInProgress, true)
                                                                                                                           .Tuning(r => r.FetchSize, command.FetchSize)),
                                                     dsl => dsl.ForwardToAction(r => r.Date, query => query.Date.ShouldBeDate(DateTime.UtcNow)),
                                                     new[] { Pleasure.Generator.Invent<GetExpectedDelayToSchedulerQuery.Response>(dsl => dsl.Tuning(s => s.Id, "2").Tuning(r => r.Instance, instance1)) }.ToList())
                                          .StubPush(instance1)
                                          .StubPush(instance2)
                                          .StubPush<ChangeDelayToSchedulerStatusCommand>(dsl => dsl.Tuning(r => r.Id, "1")
                                                                                                   .Tuning(r => r.Description, null)
                                                                                                   .Tuning(r => r.Status, DelayOfStatus.InProgress))
                                          .StubPush<ChangeDelayToSchedulerStatusCommand>(dsl => dsl.Tuning(r => r.Id, "2")
                                                                                                   .Tuning(r => r.Description, null)
                                                                                                   .Tuning(r => r.Status, DelayOfStatus.InProgress));
                              };

        Because of = () => { mockMessage.Execute(); };

        It should_be_pushed = () => { mockMessage.ShouldBePushed(); };

        #region Fake classesS

        public class FakeCommand : CommandBase
        {
            // ReSharper disable UnusedMember.Global

            #region Properties

            public string Prop { get; set; }

            #endregion

            // ReSharper restore UnusedMember.Global
            protected override void Execute()
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region Establish value

        static FakeCommand instance1;

        static FakeCommand instance2;

        static MockMessage<StartSchedulerCommand, object> mockMessage;

        #endregion
    }
}