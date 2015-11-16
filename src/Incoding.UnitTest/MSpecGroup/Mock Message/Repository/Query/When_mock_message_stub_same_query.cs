namespace Incoding.UnitTest.MSpecGroup
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using Incoding.Block;
    using Incoding.CQRS;
    using Incoding.Data;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(MockMessage<,>))]
    public class When_mock_message_stub_same_query
    {
        #region Fake classes

        class FakeEntity : IncEntityBase { }

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

                result.AddRange(Repository.Query(whereSpecification: new EntitySpec1(Param)));
                result.AddRange(Repository.Query(whereSpecification: new EntitySpec2(Param2)));
                result.AddRange(Repository.Query(whereSpecification: new EntitySpec2(Param3)));

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

        static FakeEntity[] entities;

        #endregion

        Establish establish = () =>
                              {
                                  var message = Pleasure.Generator.Invent<FakeMockMessage>();

                                  entities = new[]
                                             {
                                                     Pleasure.Generator.Invent<FakeEntity>(), 
                                                     Pleasure.Generator.Invent<FakeEntity>(), 
                                                     Pleasure.Generator.Invent<FakeEntity>()
                                             };
                                  mockMessage = MockQuery<FakeMockMessage, List<FakeEntity>>
                                          .When(message)
                                          .StubQuery(whereSpecification: new EntitySpec1(message.Param), 
                                                     entities: entities[0])
                                          .StubQuery(whereSpecification: new EntitySpec2(message.Param2), 
                                                     entities: entities[1])
                                          .StubQuery(whereSpecification: new EntitySpec2(message.Param3), 
                                                     entities: entities[2]);
                              };

        Because of = () => mockMessage.Original.Execute();

        It should_be_result = () => mockMessage.ShouldBeIsResult(list => list.ShouldEqualWeak(entities));
    }
}