namespace Incoding.UnitTest.MSpecGroup
{
    #region << Using >>

    using System;
    using Incoding.CQRS;
    using Incoding.MSpecContrib;
    using Machine.Specifications;
    using Machine.Specifications.Annotations;
    using Moq;
    using It = Machine.Specifications.It;

    #endregion

    [Subject(typeof(IncodingMoqExtensions))]
    public class When_incoding_moq_extensions_dispatcher
    {
        #region Fake classes

        class FakeCommand : CommandBase
        {
            #region Properties

            public string Prop1 { get; set; }

            #endregion

            protected override void Execute() { }
        }

        #endregion

        It should_be_stub_push = () =>
                                 {
                                     var command = Pleasure.Generator.Invent<FakeCommand>();
                                     var dispatcher = Pleasure.MockStrict<IDispatcher>();
                                     dispatcher.StubPush(command);
                                     Catch
                                             .Exception(() => dispatcher.Object.Push(command))
                                             .ShouldBeNull();
                                 };

        It should_not_be_stub_push = () =>
                                     {
                                         var command = Pleasure.Generator.Invent<FakeCommand>();
                                         var dispatcher = Pleasure.MockStrict<IDispatcher>();
                                         dispatcher.StubPush(command);
                                         Catch
                                                 .Exception(() => dispatcher.Object.Push(Pleasure.Generator.Invent<FakeCommand>()))
                                                 .ShouldNotBeNull();
                                     };

        It should_be_push = () =>
                            {
                                var dispatcher = Pleasure.Mock<IDispatcher>();
                                var command = Pleasure.Generator.Invent<FakeCommand>();

                                dispatcher.Object.Push(command);

                                Catch
                                        .Exception(() => dispatcher.ShouldBePush(command))
                                        .ShouldBeNull();
                            };

        It should_be_push_with_setting = () =>
                                         {
                                             var dispatcher = Pleasure.Mock<IDispatcher>();
                                             var command = Pleasure.Generator.Invent<FakeCommand>();
                                             var setting = Pleasure.Generator.Invent<MessageExecuteSetting>();

                                             dispatcher.Object.Push(command, setting);

                                             Catch
                                                     .Exception(() => dispatcher.ShouldBePush(command, setting))
                                                     .ShouldBeNull();
                                         };

        It should_be_push_with_null_setting = () =>
                                              {
                                                  var dispatcher = Pleasure.Mock<IDispatcher>();
                                                  var command = Pleasure.Generator.Invent<FakeCommand>();

                                                  dispatcher.Object.Push(command, Pleasure.Generator.Invent<MessageExecuteSetting>(dsl => dsl.MuteCtor()));

                                                  Catch
                                                          .Exception(() => dispatcher.ShouldBePush(command))
                                                          .ShouldBeOfType<MockException>();
                                              };

        It should_be_push_with_wrong_setting = () =>
                                               {
                                                   var dispatcher = Pleasure.Mock<IDispatcher>();
                                                   var command = Pleasure.Generator.Invent<FakeCommand>();

                                                   dispatcher.Object.Push(command, Pleasure.Generator.Invent<MessageExecuteSetting>(dsl => dsl.MuteCtor()));

                                                   Catch
                                                           .Exception(() => dispatcher.ShouldBePush(command, Pleasure.Generator.Invent<MessageExecuteSetting>(dsl => dsl.MuteCtor())))
                                                           .ShouldBeOfType<MockException>();
                                               };

        It should_be_push_composite = () =>
                                      {
                                          var dispatcher = Pleasure.Mock<IDispatcher>();
                                          var command = Pleasure.Generator.Invent<FakeCommand>();
                                          var setting = Pleasure.Generator.Invent<MessageExecuteSetting>();

                                          dispatcher.Object.Push(composite =>
                                                                 {
                                                                     composite.Quote(command, setting);
                                                                     composite.Quote(command, setting);
                                                                 });

                                          Catch
                                                  .Exception(() => dispatcher.ShouldBePush(command, callCount: 2, executeSetting: setting))
                                                  .ShouldBeNull();
                                      };

        It should_be_push_composite_different_command = () =>
                                                        {
                                                            var dispatcher = Pleasure.Mock<IDispatcher>();
                                                            var command = Pleasure.Generator.Invent<FakeCommand>();
                                                            var command2 = Pleasure.Generator.Invent<Fake2Command>();
                                                            var setting = Pleasure.Generator.Invent<MessageExecuteSetting>();

                                                            dispatcher.Object.Push(composite =>
                                                                                   {
                                                                                       composite.Quote(command, setting);
                                                                                       composite.Quote(command2, setting);
                                                                                   });

                                                            Catch.Exception(() => dispatcher.ShouldBePush(command, executeSetting: setting)).ShouldBeNull();
                                                            Catch.Exception(() => dispatcher.ShouldBePush(command2, executeSetting: setting)).ShouldBeNull();
                                                        };

        It should_be_push_composite_without_setting = () =>
                                                      {
                                                          var dispatcher = Pleasure.Mock<IDispatcher>();
                                                          var command = Pleasure.Generator.Invent<FakeCommand>();

                                                          dispatcher.Object.Push(composite => composite.Quote(command));

                                                          Catch
                                                                  .Exception(() => dispatcher.ShouldBePush(command, new MessageExecuteSetting
                                                                                                                    {
                                                                                                                            DataBaseInstance = Pleasure.Generator.String()
                                                                                                                    }))
                                                                  .ShouldBeOfType<MockException>();
                                                      };

        It should_be_push_composite_with_setting = () =>
                                                   {
                                                       var dispatcher = Pleasure.Mock<IDispatcher>();
                                                       var command = Pleasure.Generator.Invent<FakeCommand>();
                                                       var setting = Pleasure.Generator.Invent<MessageExecuteSetting>(dsl => dsl.Tuning(r => r.Connection, @"Data Source=Work\SQLEXPRESS;Database=IncRealDb;Integrated Security=true;"));

                                                       dispatcher.Object.Push(composite => composite.Quote(command, setting));

                                                       Catch
                                                               .Exception(() => dispatcher.ShouldBePush(command, setting))
                                                               .ShouldBeNull();
                                                   };

        It should_be_push_with_wrong_connection = () =>
                                                  {
                                                      var dispatcher = Pleasure.Mock<IDispatcher>();
                                                      const string sqlConnection = @"Data Source=Work\SQLEXPRESS;Database=IncRealDb;Integrated Security=true;";

                                                      dispatcher.Object.Push(new FakeCommand(), setting => setting.Connection = sqlConnection);

                                                      Catch
                                                              .Exception(() => dispatcher.ShouldBePush(new FakeCommand(), new MessageExecuteSetting
                                                                                                                          {
                                                                                                                                  Connection = @"Data Source=any;Database=different;"
                                                                                                                          }))
                                                              .ShouldBeOfType<MockException>();
                                                  };

        It should_not_be_push = () =>
                                {
                                    var dispatcher = Pleasure.Mock<IDispatcher>();
                                    var command = Pleasure.Generator.Invent<FakeCommand>();

                                    dispatcher.Object.Push(command);

                                    Catch.Exception(() => dispatcher.ShouldBePush(Pleasure.Generator.Invent<FakeCommand>(), callCount: 0)).ShouldBeNull();
                                };

        It should_not_be_push_with_composite = () =>
                                               {
                                                   var dispatcher = Pleasure.Mock<IDispatcher>();
                                                   var command = Pleasure.Generator.Invent<FakeCommand>();
                                                   var commandComposite = new CommandComposite();
                                                   commandComposite.Quote(command);
                                                   commandComposite.Quote(command);

                                                   dispatcher.Object.Push(commandComposite);

                                                   Catch
                                                           .Exception(() => dispatcher.ShouldBePush(Pleasure.Generator.Invent<FakeCommand>(), callCount: 0))
                                                           .ShouldBeNull();
                                               };

        It should_stub_push_as_throw = () =>
                                       {
                                           var dispatcher = Pleasure.Mock<IDispatcher>();
                                           var command = Pleasure.Generator.Invent<FakeCommand>();

                                           dispatcher.StubPushAsThrow(command, new IncFakeException());

                                           Catch.Exception(() => dispatcher.Object.Push(command)).ShouldBeOfType<IncFakeException>();
                                       };

        It should_stub_push_as_throw_without_push = () =>
                                                    {
                                                        var dispatcher = Pleasure.Mock<IDispatcher>();
                                                        var command = Pleasure.Generator.Invent<FakeCommand>();

                                                        dispatcher.StubPushAsThrow(command, new IncFakeException());

                                                        Catch.Exception(() => dispatcher.Object.Push(command.Inverse())).ShouldBeNull();
                                                    };

        It should_stub_push_as_throw_on_composite = () =>
                                                    {
                                                        var dispatcher = Pleasure.Mock<IDispatcher>();
                                                        var command = Pleasure.Generator.Invent<FakeCommand>();

                                                        dispatcher.StubPushAsThrow(command, new IncFakeException());

                                                        Catch.Exception(() => dispatcher.Object.Push(composite =>
                                                                                                     {
                                                                                                         composite.Quote(Pleasure.Generator.Invent<FakeCommand>());
                                                                                                         composite.Quote(command);
                                                                                                     })).ShouldBeOfType<IncFakeException>();
                                                    };

        It should_stub_push_as_throw_with_composite = () =>
                                                      {
                                                          var dispatcher = Pleasure.Mock<IDispatcher>();
                                                          var command = Pleasure.Generator.Invent<FakeCommand>();
                                                          var setting = Pleasure.Generator.Invent<MessageExecuteSetting>();

                                                          dispatcher.StubPushAsThrow(command, new IncFakeException(), setting);

                                                          var commandComposite = new CommandComposite();
                                                          commandComposite.Quote(command, setting);
                                                          Catch
                                                                  .Exception(() => dispatcher.Object.Push(commandComposite))
                                                                  .ShouldBeOfType<IncFakeException>();
                                                      };
    }

    internal class Fake2Command : CommandBase
    {
        #region Properties

        [UsedImplicitly]
        public string AnotherProp { get; set; }

        #endregion

        protected override void Execute()
        {
            throw new NotImplementedException();
        }
    }
}