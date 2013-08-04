namespace Incoding.UnitTest.MSpecGroup
{
    #region << Using >>

    using System.Collections.Generic;
    using System.Linq;
    using Incoding.CQRS;
    using Incoding.Data;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(MockMessage<,>))]
    public class When_mock_message_stub_query_paginated_specification_wrong
    {
        #region Fake classes

        class FakeMockMessage : QueryBase<List<IEntity>>
        {
            #region Properties

            public int CurrentPage { get; set; }

            public int PageSize { get; set; }

            #endregion

            #region Override

            protected override List<IEntity> ExecuteResult()
            {
                return Repository.Query<IEntity>(paginatedSpecification: new PaginatedSpecification(CurrentPage, PageSize)).ToList();
            }

            #endregion
        }

        #endregion

        #region Estabilish value

        static MockMessage<FakeMockMessage, List<IEntity>> mockMessage;

        #endregion

        Establish establish = () =>
                                  {
                                      var query = Pleasure.Generator.Invent<FakeMockMessage>();
                                      mockMessage = MockQuery<FakeMockMessage, List<IEntity>>
                                              .When(query)
                                              .StubQuery(paginatedSpecification: new PaginatedSpecification(Pleasure.Generator.PositiveNumber(), Pleasure.Generator.PositiveNumber()),
                                                         entities: Pleasure.MockAsObject<IEntity>());
                                  };

        Because of = () => mockMessage.Original.Execute();

        It should_be_empty_result = () => mockMessage.ShouldBeIsResult(list => list.ShouldBeEmpty());
    }
}