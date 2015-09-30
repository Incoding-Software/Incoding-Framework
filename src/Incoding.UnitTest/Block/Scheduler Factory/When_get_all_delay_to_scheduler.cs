namespace Incoding.UnitTest
{
    #region << Using >>

    using System.Collections.Generic;
    using Incoding.Block;
    using Incoding.Data;
    using Incoding.Extensions;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(GetExpectedDelayToSchedulerQuery))]
    public class When_get_all_delay_to_scheduler
    {
        Establish establish = () =>
                              {
                                  var query = Pleasure.Generator.Invent<GetExpectedDelayToSchedulerQuery>();
                                  uniqueKey = Pleasure.Generator.String();
                                  successEntities = new[]
                                                    {
                                                            Pleasure.Generator.Invent<DelayToScheduler>(dsl => dsl.Tuning(r => r.GroupKey, Pleasure.Generator.TheSameString())),
                                                            Pleasure.Generator.Invent<DelayToScheduler>(dsl => dsl.Tuning(r => r.GroupKey, uniqueKey)),
                                                    };
                                  errorEntities = new[]
                                                  {
                                                          Pleasure.Generator.Invent<DelayToScheduler>(dsl => dsl.Tuning(r => r.GroupKey, Pleasure.Generator.TheSameString())),
                                                  };

                                  mockQuery = MockQuery<GetExpectedDelayToSchedulerQuery, Dictionary<string, List<DelayToScheduler>>>
                                          .When(query)
                                          .StubQuery(whereSpecification: new DelayToSchedulerByStatusWhere(DelayOfStatus.New)
                                                             .And(new DelayToSchedulerAvailableStartsOnWhereSpec(query.Date)),
                                                     paginatedSpecification: new PaginatedSpecification(1, query.FetchSize),
                                                     entities: successEntities)
                                          .StubQuery(whereSpecification: new DelayToSchedulerByStatusWhere(DelayOfStatus.Error)
                                                             .And(new DelayToSchedulerAvailableStartsOnWhereSpec(query.Date)),
                                                     paginatedSpecification: new PaginatedSpecification(1, 10),
                                                     entities: errorEntities);
                              };

        Because of = () => mockQuery.Original.Execute();

        It should_be_result = () => mockQuery.ShouldBeIsResult(dictionary =>
                                                               {
                                                                   dictionary[Pleasure.Generator.TheSameString()].ShouldEqualWeakEach(new[]
                                                                                                                                      {
                                                                                                                                              successEntities[0],
                                                                                                                                              errorEntities[0],
                                                                                                                                      });

                                                                   dictionary[uniqueKey].ShouldEqualWeakEach(new[] { successEntities[1] });
                                                               });

        #region Establish value

        static MockMessage<GetExpectedDelayToSchedulerQuery, Dictionary<string, List<DelayToScheduler>>> mockQuery;

        static DelayToScheduler[] successEntities;

        static string uniqueKey;

        static DelayToScheduler[] errorEntities;

        #endregion
    }
}