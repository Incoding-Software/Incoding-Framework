namespace Incoding.UnitTest
{
    #region << Using >>

    using System.Collections.Generic;
    using Incoding.Block;
    using Incoding.Data;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(GetExpectedDelayToSchedulerQuery))]
    public class When_get_all_delay_to_scheduler
    {
        #region Establish value

        static MockMessage<GetExpectedDelayToSchedulerQuery, Dictionary<string, List<DelayToScheduler>>> mockQuery;

        static DelayToScheduler[] entities;

        static string uniqueKey;

        #endregion

        Establish establish = () =>
                                  {
                                      var query = Pleasure.Generator.Invent<GetExpectedDelayToSchedulerQuery>();
                                      uniqueKey = Pleasure.Generator.String();
                                      entities = new[]
                                                     {
                                                             Pleasure.Generator.Invent<DelayToScheduler>(dsl => dsl.Tuning(r => r.GroupKey, Pleasure.Generator.TheSameString())),
                                                             Pleasure.Generator.Invent<DelayToScheduler>(dsl => dsl.Tuning(r => r.GroupKey, uniqueKey)),
                                                             Pleasure.Generator.Invent<DelayToScheduler>(dsl => dsl.Tuning(r => r.GroupKey, Pleasure.Generator.TheSameString())),
                                                     };

                                      mockQuery = MockQuery<GetExpectedDelayToSchedulerQuery, Dictionary<string, List<DelayToScheduler>>>
                                              .When(query)
                                              .StubQuery(whereSpecification: !new DelayToSchedulerByStatusWhere(DelayOfStatus.Success),
                                                         paginatedSpecification: new PaginatedSpecification(1, query.FetchSize),
                                                         entities: entities);
                                  };

        Because of = () => mockQuery.Original.Execute();

        It should_be_result = () => mockQuery.ShouldBeIsResult(dictionary =>
                                                                   {
                                                                       dictionary[Pleasure.Generator.TheSameString()].ShouldEqualWeakEach(new[]
                                                                                                                                              {
                                                                                                                                                      entities[0],
                                                                                                                                                      entities[2],
                                                                                                                                              });

                                                                       dictionary[uniqueKey].ShouldEqualWeakEach(new[] { entities[1] });
                                                                   });
    }
}