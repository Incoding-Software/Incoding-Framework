namespace Incoding.UnitTest.MSpecGroup
{
    #region << Using >>

    using System;
    using System.Linq.Expressions;
    using Incoding.CQRS;
    using Incoding.Data;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(MockMessage<,>))]
    public class When_mock_message_stub_paginated
    {
        #region Fake classes

        public class FakeEntity : IncEntityBase { }

        class FakeMockMessage : QueryBase<IncPaginatedResult<FakeEntity>>
        {
            #region Properties

            public string Param { get; set; }

            #endregion

            #region Override

            protected override IncPaginatedResult<FakeEntity> ExecuteResult()
            {
                return Repository.Paginated(new PaginatedSpecification(1, 10), 
                                            fetchSpecification: new EntityFetchSpecification(), 
                                            orderSpecification: new FakeOrderSpecification(), 
                                            whereSpecification: new EntitySpec1(Param));
            }

            #endregion
        }

        class EntityFetchSpecification : FetchSpecification<FakeEntity>
        {
            public override Action<AdHocFetchSpecification<FakeEntity>> FetchedBy()
            {
                return specification => specification.Join(r => r.Id);
            }
        }

        class FakeOrderSpecification : OrderSpecification<FakeEntity>
        {
            public override Action<AdHocOrderSpecification<FakeEntity>> SortedBy()
            {
                return specification => specification.OrderBy(r => r.Id);
            }
        }

        class EntitySpec1 : Specification<FakeEntity>
        {
            #region Fields

            readonly string param;

            #endregion

            #region Constructors

            public EntitySpec1(string param)
            {
                this.param = param;
            }

            #endregion

            public override Expression<Func<FakeEntity, bool>> IsSatisfiedBy()
            {
                return r => r.Id == this.param;
            }
        }

        #endregion

        #region Estabilish value

        static MockMessage<FakeMockMessage, IncPaginatedResult<FakeEntity>> mockMessage;

        #endregion

        Establish establish = () =>
                                  {
                                      var fakeMockMessage = Pleasure.Generator.Invent<FakeMockMessage>();

                                      mockMessage = MockQuery<FakeMockMessage, IncPaginatedResult<FakeEntity>>
                                              .When(fakeMockMessage)
                                              .StubPaginated(new PaginatedSpecification(1, 10), 
                                                             fetchSpecification: new EntityFetchSpecification(), 
                                                             orderSpecification: new FakeOrderSpecification(), 
                                                             whereSpecification: new EntitySpec1(fakeMockMessage.Param), 
                                                             result: new IncPaginatedResult<FakeEntity>(Pleasure.ToList(Pleasure.Generator.Invent<FakeEntity>()), 1));
                                  };

        Because of = () => mockMessage.Original.Execute();

        It should_be_empty_result = () => mockMessage.ShouldBeIsResult(result =>
                                                                           {
                                                                               result.TotalCount.ShouldEqual(1);
                                                                               result.Items.Count.ShouldEqual(1);
                                                                           });
    }
}