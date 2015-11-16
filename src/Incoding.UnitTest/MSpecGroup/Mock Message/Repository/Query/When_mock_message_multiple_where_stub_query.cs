namespace Incoding.UnitTest.MSpecGroup
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using Incoding.Block;
    using Incoding.CQRS;
    using Incoding.Data;
    using Incoding.Extensions;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(MockMessage<,>))]
    public class When_mock_message_multiple_where_stub_query
    {
        #region Fake classes

        class FakeEntity : IncEntityBase { }

        class FakeFetchSpecification : FetchSpecification<FakeEntity>
        {
            public override Action<AdHocFetchSpecificationBase<FakeEntity>> FetchedBy()
            {
                return specification => specification.Join(r => r.Id);
            }
        }

        class FakeMockMessage : QueryBase<List<FakeEntity>>
        {
            #region Properties

            public string Param { get; set; }

            public string Param2 { get; set; }

            public string Param3 { get; set; }

            #endregion

            #region Override

            protected override List<FakeEntity> ExecuteResult()
            {
                var result = new List<FakeEntity>();

                result.AddRange(Repository.Query(fetchSpecification: new FakeFetchSpecification(), 
                                                 whereSpecification: new EntitySpec1(Param)
                                                         .And(new EntitySpec1(Param2))
                                                         .And(new EntitySpec2(Param3))));

                return result;
            }

            #endregion
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
                return r => r.Id == param;
            }
        }

        class EntitySpec2 : Specification<FakeEntity>
        {
            #region Fields

            readonly string param;

            #endregion

            #region Constructors

            public EntitySpec2(string param)
            {
                this.param = param;
            }

            #endregion

            public override Expression<Func<FakeEntity, bool>> IsSatisfiedBy()
            {
                return r => r.Id == param;
            }
        }

        #endregion

        #region Establish value

        static MockMessage<FakeMockMessage, List<FakeEntity>> mockMessage;

        #endregion

        Establish establish = () =>
                              {
                                  var fakeMockMessage = Pleasure.Generator.Invent<FakeMockMessage>();

                                  mockMessage = MockQuery<FakeMockMessage, List<FakeEntity>>
                                          .When(fakeMockMessage)
                                          .StubQuery(fetchSpecification: new FakeFetchSpecification(), 
                                                     whereSpecification: new EntitySpec1(fakeMockMessage.Param)
                                                             .And(new EntitySpec1(fakeMockMessage.Param2))
                                                             .And(new EntitySpec2(fakeMockMessage.Param3)), 
                                                     entities: Pleasure.Generator.Invent<FakeEntity>());
                              };

        Because of = () => mockMessage.Original.Execute();

        It should_be_result = () => mockMessage.ShouldBeIsResult(list => list.Count.ShouldEqual(1));
    }
}