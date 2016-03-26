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
    public class When_start_scheduler_with_error
    {
        #region Fake classes

        class FakeCommand : CommandBase
        {
            #region Properties

            public string Prop { get; set; }

            #endregion

            protected override void Execute()
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region Establish value

        static bool isStart = true;

        static Exception exception;

        static FakeCommand errorInstance;

        static FakeCommand successInstance;

        static ArgumentException expectedEx;

        static List<GetExpectedDelayToSchedulerQuery.Response> response;

        static MockMessage<StartSchedulerCommand, object> mockMessage;

        #endregion

        Cleanup cleanup = () => isStart = false;

        Establish establish = () =>
                              {
                                  errorInstance = Pleasure.Generator.Invent<FakeCommand>(dsl => dsl.GenerateTo(r => r.Setting));
                                  successInstance = Pleasure.Generator.Invent<FakeCommand>(dsl => dsl.GenerateTo(r => r.Setting));
                                  response = new List<GetExpectedDelayToSchedulerQuery.Response>()
                                             {
                                                     Pleasure.Generator.Invent<GetExpectedDelayToSchedulerQuery.Response>(dsl => dsl.Tuning(r => r.Instance, successInstance)), 
                                                     Pleasure.Generator.Invent<GetExpectedDelayToSchedulerQuery.Response>(dsl => dsl.Tuning(r => r.Instance, errorInstance))
                                             };

                                  expectedEx = new ArgumentException(Pleasure.Generator.String());

                                  var command = Pleasure.Generator.Invent<StartSchedulerCommand>(dsl => dsl.Tuning(r => r.Conditional, () => isStart)
                                                                                                           .Tuning(r => r.FetchSize, Pleasure.Generator.TheSameNumber()));

                                  mockMessage = MockCommand<StartSchedulerCommand>
                                          .When(command)
                                          .StubPushAsThrow(errorInstance, expectedEx, errorInstance.Setting)
                                          .StubQuery(new GetExpectedDelayToSchedulerQuery { FetchSize = Pleasure.Generator.TheSameNumber() }, 
                                                     dsl => dsl.ForwardToAction(s => s.Date, r => r.Date.ShouldBeDate(DateTime.UtcNow)), 
                                                     response);
                              };

        Because of = () =>
                     {
                         exception = Catch.Exception(() => mockMessage.Execute());
                         Thread.Sleep(3.Seconds());
                     };

        It should_be_not_stop = () => exception.ShouldBeNull();
    }
}