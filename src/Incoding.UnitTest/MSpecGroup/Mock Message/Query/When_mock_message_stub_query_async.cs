using System.Threading.Tasks;

namespace Incoding.UnitTest.MSpecGroup
{
    #region << Using >>

    using System;
    using Incoding.CQRS;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(MockMessage<,>))]
    public class When_mock_message_stub_query_async
    {
        #region Fake classes

        class FakeQuery : QueryBase<string>
        {
            #region Properties

            public string Id { get; set; }

            #endregion

            protected override string ExecuteResult()
            {
                throw new NotImplementedException();
            }
        }

        class FakeQueryWithoutProperties : QueryBase<string>
        {
            protected override string ExecuteResult()
            {
                throw new NotImplementedException();
            }
        }

        class FakeMockMessage : CommandBase
        {
            #region Override

            public string Id { get; set; }

            #endregion

            protected override void Execute()
            {
                Task<string> result = Dispatcher.Async().Query(new FakeQuery { Id = Id });
                result.Wait();
                Result = result.Result;
            }
        }

        class FakeMockMessage2 : CommandBase
        {
            protected override void Execute()
            {
                Task<string> result = Dispatcher.Async().Query(new FakeQueryWithoutProperties());
                result.Wait();
                Result = result.Result;
            }
        }

        #endregion

        It should_be_stub = () =>
                            {
                                var input = Pleasure.Generator.Invent<FakeMockMessage>();
                                var mockMessage = MockCommand<FakeMockMessage>
                                        .When(input)
                                        .StubQuery(Pleasure.Generator.Invent<FakeQuery>(dsl => dsl.Tuning(r => r.Id, input.Id)), 
                                                Pleasure.Generator.TheSameString());
                                mockMessage.Original.Execute();
                                mockMessage.ShouldBeIsResult(Pleasure.Generator.TheSameString());
                            };

        It should_be_stub_as_invent = () =>
                                      {
                                          var input = Pleasure.Generator.Invent<FakeMockMessage2>();
                                          var mockMessage = MockCommand<FakeMockMessage2>
                                                  .When(input)
                                                  .StubQuery<FakeQueryWithoutProperties, string>(Pleasure.Generator.TheSameString());
                                          mockMessage.Original.Execute();
                                          mockMessage.ShouldBeIsResult(Pleasure.Generator.TheSameString());
                                      };

        It should_be_stub_as_null = () =>
                                    {
                                        var input = Pleasure.Generator.Invent<FakeMockMessage2>();
                                        var mockMessage = MockCommand<FakeMockMessage2>
                                                .When(input)
                                                .StubQueryAsNull<FakeQueryWithoutProperties, string>();
                                        mockMessage.Original.Execute();
                                        mockMessage.ShouldBeIsResult(o => o.ShouldBeNull());
                                    };

        It should_be_stub_as_invent_dsl = () =>
                                          {
                                              var input = Pleasure.Generator.Invent<FakeMockMessage>();
                                              var mockMessage = MockCommand<FakeMockMessage>
                                                      .When(input)
                                                      .StubQuery<FakeQuery, string>(dsl => dsl.Tuning(r => r.Id, input.Id), 
                                                              Pleasure.Generator.TheSameString());
                                              mockMessage.Original.Execute();
                                              mockMessage.ShouldBeIsResult(Pleasure.Generator.TheSameString());
                                          };
    }
}