namespace Incoding.UnitTest
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Threading;
    using Incoding.Block;
    using Incoding.Block.IoC;
    using Incoding.CQRS;
    using Incoding.Extensions;
    using Incoding.MSpecContrib;
    using Machine.Specifications;
    using Moq;
    using It = Machine.Specifications.It;

    #endregion

    [Subject(typeof(SchedulerFactory))]
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

        static SchedulerFactory scheduler;

        static Mock<IDispatcher> dispatcher;

        static FakeCommand instance1;

        static FakeCommand instance2;

        static bool isStart = true;

        static MessageExecuteSetting schedulerSetting;

        #endregion

        Cleanup cleanup = () => isStart = false;

        Establish establish = () =>
                              {
                                  schedulerSetting = Pleasure.Generator.Invent<MessageExecuteSetting>();
                                  var response = new Dictionary<string, List<DelayToScheduler>>();
                                  instance1 = Pleasure.Generator.Invent<FakeCommand>(dsl => dsl.GenerateTo(r => r.Setting));
                                  instance2 = Pleasure.Generator.Invent<FakeCommand>(dsl => dsl.GenerateTo(r => r.Setting));
                                  response.Add(Pleasure.Generator.String(), new List<DelayToScheduler>
                                                                            {
                                                                                    Pleasure.MockAsObject<DelayToScheduler>(mock =>
                                                                                                                            {
                                                                                                                                mock.SetupGet(r => r.Id).Returns(Pleasure.Generator.TheSameString());
                                                                                                                                mock.SetupGet(r => r.Instance).Returns(instance1);
                                                                                                                            }), 
                                                                                    Pleasure.MockAsObject<DelayToScheduler>(mock =>
                                                                                                                            {
                                                                                                                                mock.SetupGet(r => r.Id).Returns(Pleasure.Generator.TheSameNumber().ToString());
                                                                                                                                mock.SetupGet(r => r.Instance).Returns(instance2);
                                                                                                                            })
                                                                            });

                                  dispatcher = Pleasure.Mock<IDispatcher>();
                                  var schedulerQuery = new GetExpectedDelayToSchedulerQuery { FetchSize = Pleasure.Generator.TheSameNumber() };
                                  dispatcher.StubQuery(schedulerQuery, dsl => dsl.ForwardToAction(r => r.Date, query => query.Date.ShouldBeDate(DateTime.UtcNow)), response, schedulerSetting);
                                  IoCFactory.Instance.StubTryResolve(dispatcher.Object);
                              };

        Because of = () =>
                     {
                         SchedulerFactory.Instance.Initialize(initScheduler =>
                                                              {
                                                                  initScheduler.FetchSize = Pleasure.Generator.TheSameNumber();
                                                                  initScheduler.Conditional = () => isStart;
                                                                  initScheduler.Setting = schedulerSetting;
                                                              });
                         Thread.Sleep(2.Seconds());
                     };

        It should_be_change_to_progress = () => dispatcher.ShouldBePush(new ChangeDelayToSchedulerStatusCommand
                                                                        {
                                                                                Ids = new[]
                                                                                      {
                                                                                              Pleasure.Generator.TheSameString(), 
                                                                                              Pleasure.Generator.TheSameNumber().ToString()
                                                                                      }, 
                                                                                Status = DelayOfStatus.InProgress
                                                                        }, schedulerSetting);

        It should_be_change_to_success = () => dispatcher.ShouldBePush(new ChangeDelayToSchedulerStatusCommand
                                                                       {
                                                                               Ids = new[]
                                                                                     {
                                                                                             Pleasure.Generator.TheSameString(), 
                                                                                             Pleasure.Generator.TheSameNumber().ToString()
                                                                                     }, 
                                                                               Status = DelayOfStatus.Success
                                                                       }, schedulerSetting);

        It should_be_push_instance_1 = () => dispatcher.ShouldBePush(instance1, instance1.Setting);

        It should_be_push_instance_2 = () => dispatcher.ShouldBePush(instance2, instance2.Setting);
    }
}