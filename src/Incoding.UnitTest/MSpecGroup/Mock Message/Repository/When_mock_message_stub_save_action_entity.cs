using System;

namespace Incoding.UnitTest.MSpecGroup
{
    #region << Using >>

    using System.Collections.Generic;
    using Incoding.Block;
    using Incoding.CQRS;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(MockMessage<,>))]
    public class When_mock_message_stub_save_action_entity
    {
        #region Fake classes

        class FakeEntityWithDate : FakeEntityForNew
        {
            public new string Id { get; set; }
            public DateTime Date { get; set; }
        }

        class FakeMockMessage : QueryBase<IncStructureResponse<string>>
        {
            #region Override

            protected override IncStructureResponse<string> ExecuteResult()
            {
                var fakeEntityForNew = new FakeEntityWithDate
                {
                    Date = DateTime.Now
                };
                Repository.Save(fakeEntityForNew);
                return new IncStructureResponse<string>(fakeEntityForNew.Id);
            }

            #endregion
        }

        #endregion

        #region Establish value

        static MockMessage<FakeMockMessage, IncStructureResponse<string>> mockMessage;

        #endregion

        Establish establish = () =>
                              {
                                  var input = Pleasure.Generator.Invent<FakeMockMessage>();
                                  result = Pleasure.Generator.GuidAsString();
                                  mockMessage = MockQuery<FakeMockMessage, IncStructureResponse<string>>
                                          .When(input)
                                          .StubSave<FakeEntityWithDate>(date => date.ShouldEqualWeak(new FakeEntityWithDate(), 
                                              dsl => dsl.ForwardToAction(r => r.Date, r => r.Date.ShouldBeDate(DateTime.Now))), result);
                              };

        Because of = () => mockMessage.Original.Execute();

        It should_be_result = () => mockMessage.ShouldBeIsResult(entity => entity.Value.ShouldEqualWeak(result));
        private static string result;
    }
}