namespace Incoding.UnitTest.MSpecGroup
{
    #region << Using >>

    using System;
    using Incoding.CQRS;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Behaviors]
    public class Behavior_mock_message_stub_query<TMessage, TMessage2, TMessage3>
            where TMessage : CommandBase, Behavior_mock_message_stub_query<TMessage, TMessage2, TMessage3>.IIdCommand
            where TMessage2 : CommandBase
            where TMessage3 : CommandBase
    {
        It should_be_stub = () =>
                            {
                                var input = Pleasure.Generator.Invent<TMessage>();
                                var mockMessage = MockCommand<TMessage>
                                        .When(input)                                        
                                        .StubQuery(Pleasure.Generator.Invent<FakeQuery>(dsl => dsl.Tuning(r => r.Id, input.Id)),
                                                   Pleasure.Generator.TheSameString());
                                mockMessage.Execute();
                                mockMessage.ShouldBeIsResult(Pleasure.Generator.TheSameString());
                            };

        It should_be_stub_as_invent = () =>
                                      {
                                          var input = Pleasure.Generator.Invent<TMessage2>();
                                          var mockMessage = MockCommand<TMessage2>
                                                  .When(input)
                                                  .StubQuery<FakeQueryWithoutProperties, string>(Pleasure.Generator.TheSameString());
                                          mockMessage.Execute();
                                          mockMessage.ShouldBeIsResult(Pleasure.Generator.TheSameString());
                                      };

        It should_be_stub_as_invent_dsl = () =>
                                          {
                                              var input = Pleasure.Generator.Invent<TMessage>();
                                              var mockMessage = MockCommand<TMessage>
                                                      .When(input)
                                                      .StubQuery<FakeQuery, string>(dsl => dsl.Tuning(r => r.Id, input.Id),
                                                                                    Pleasure.Generator.TheSameString());
                                              mockMessage.Execute();
                                              mockMessage.ShouldBeIsResult(Pleasure.Generator.TheSameString());
                                          };

        It should_be_stub_as_null = () =>
                                    {
                                        var input = Pleasure.Generator.Invent<TMessage2>();
                                        var mockMessage = MockCommand<TMessage2>
                                                .When(input)
                                                .StubQueryAsNull<FakeQueryWithoutProperties, string>();
                                        mockMessage.Execute();
                                        mockMessage.ShouldBeIsResult(o => o.ShouldBeNull());
                                    };

        It should_be_stub_same_query = () =>
                                       {
                                           var input = Pleasure.Generator.Invent<TMessage3>();
                                           var mockMessage = MockCommand<TMessage3>
                                                   .When(input)
                                                   .StubQuery<FakeQuery, string>(dsl => dsl.Tuning(r => r.Id, "1"), "The")
                                                   .StubQuery<FakeQuery, string>(dsl => dsl.Tuning(r => r.Id, "2"), "SameString");
                                           mockMessage.Execute();
                                           mockMessage.ShouldBeIsResult(o => o.ShouldEqual(Pleasure.Generator.TheSameString()));
                                       };

        public interface IIdCommand
        {
            string Id { get; set; }
        }

        protected class FakeQuery : QueryBase<string>
        {
            #region Properties

            public string Id { get; set; }

            #endregion

            protected override string ExecuteResult()
            {
                throw new NotImplementedException();
            }
        }

        protected class FakeQueryWithoutProperties : QueryBase<string>
        {
            protected override string ExecuteResult()
            {
                throw new NotImplementedException();
            }
        }
    }
}