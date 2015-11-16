namespace Incoding.UnitTest
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Threading;
    using Incoding.Block;
    using Incoding.CQRS;
    using Incoding.Extensions;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(StartSchedulerCommand))]
    public class When_scheduler_factory_initialize_success
    {
        #region Fake classes

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

        static bool isStart = true;

        static List<GetExpectedDelayToSchedulerQuery.Response> response;

        static MockMessage<StartSchedulerCommand, object> mockMessage;

        static Exception exception;

        #endregion

        Cleanup cleanup = () => isStart = false;

        Establish establish = () =>
                              {
                                  instance1 = Pleasure.Generator.Invent<FakeCommand>(dsl => dsl.GenerateTo(r => r.Setting));
                                  instance2 = Pleasure.Generator.Invent<FakeCommand>(dsl => dsl.GenerateTo(r => r.Setting));
                                  response = new List<GetExpectedDelayToSchedulerQuery.Response>()
                                             {
                                                     Pleasure.Generator.Invent<GetExpectedDelayToSchedulerQuery.Response>(dsl => dsl.Tuning(r => r.Instance, instance1)), 
                                                     Pleasure.Generator.Invent<GetExpectedDelayToSchedulerQuery.Response>(dsl => dsl.Tuning(r => r.Instance, instance2))
                                             };

                                  var command = Pleasure.Generator.Invent<StartSchedulerCommand>(dsl => dsl.Tuning(r => r.Conditional, () => isStart)
                                                                                                           .GenerateTo(r => r.Setting));

                                  var getExpectedDelayToSchedulerQuery = Pleasure.Generator.Invent<GetExpectedDelayToSchedulerQuery>(dsl => dsl.Tuning(r => r.FetchSize, command.FetchSize));
                                  mockMessage = MockCommand<StartSchedulerCommand>
                                          .When(command)
                                          .StubQuery(getExpectedDelayToSchedulerQuery, dsl => dsl.ForwardToAction(r => r.Date, query => query.Date.ShouldBeDate(DateTime.UtcNow)), response, command.Setting)
                                          .StubPush(instance1)
                                          .StubPush(instance2)
                                          .StubPush<ChangeDelayToSchedulerStatusCommand>(dsl => dsl.Tuning(r => r.Id, response[0].Id)
                                                                                                   .Tuning(r => r.Description, null)
                                                                                                   .Tuning(r => r.Status, DelayOfStatus.InProgress))
                                          .StubPush<ChangeDelayToSchedulerStatusCommand>(dsl => dsl.Tuning(r => r.Id, response[1].Id)
                                                                                                   .Tuning(r => r.Description, null)
                                                                                                   .Tuning(r => r.Status, DelayOfStatus.InProgress));
                              };

        Because of = () =>
                     {
                         exception = Catch.Exception(() => mockMessage.Execute());
                         Thread.Sleep(2.Seconds());
                     };

        It should_be_verify = () => exception.ShouldBeNull();
    }
}  