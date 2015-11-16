namespace Incoding.UnitTest
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using Incoding.Block;
    using Incoding.CQRS;
    using Incoding.Data;
    using Incoding.Extensions;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(GetExpectedDelayToSchedulerQuery))]
    public class When_get_all_delay_to_scheduler
    {
        #region Fake classes

        public class FakeCommand : CommandBase
        {
            protected override void Execute()
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region Establish value

        static MockMessage<GetExpectedDelayToSchedulerQuery, List<GetExpectedDelayToSchedulerQuery.Response>> mockQuery;

        static DelayToScheduler[] successEntities;

        static DelayToScheduler[] errorEntities;

        static FakeCommand fakeCommand;

        #endregion

        Establish establish = () =>
                              {
                                  var query = Pleasure.Generator.Invent<GetExpectedDelayToSchedulerQuery>();
                                  fakeCommand = Pleasure.Generator.Invent<FakeCommand>();
                                  Action<IInventFactoryDsl<DelayToScheduler>> action = dsl => dsl
                                                                                                      .Tuning(r => r.Type, typeof(FakeCommand).AssemblyQualifiedName)
                                                                                                      .Tuning(r => r.Command, fakeCommand.ToJsonString());
                                  successEntities = new[]
                                                    {
                                                            Pleasure.Generator.Invent(action), 
                                                            Pleasure.Generator.Invent(action), 
                                                    };
                                  errorEntities = new[]
                                                  {
                                                          Pleasure.Generator.Invent(action), 
                                                  };

                                  mockQuery = MockQuery<GetExpectedDelayToSchedulerQuery, List<GetExpectedDelayToSchedulerQuery.Response>>
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

        It should_be_result = () => mockQuery.ShouldBeIsResult(list =>
                                                               {
                                                                   var all = new List<DelayToScheduler>(successEntities);
                                                                   all.AddRange(errorEntities);
                                                                   list.ShouldEqualWeakEach(all, (dsl, i) => dsl.ForwardToValue(r => r.Instance, fakeCommand));
                                                               });
    }
}