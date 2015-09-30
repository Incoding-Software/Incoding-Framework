namespace Incoding.Block
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Incoding.CQRS;
    using Incoding.Data;
    using Incoding.Extensions;

    #endregion

    public class GetExpectedDelayToSchedulerQuery : QueryBase<Dictionary<string, List<DelayToScheduler>>>
    {
        protected override Dictionary<string, List<DelayToScheduler>> ExecuteResult()
        {
            var all = new List<DelayToScheduler>();
            all.AddRange(Repository.Query(whereSpecification: (new DelayToSchedulerByStatusWhere(DelayOfStatus.New))
                                                  .And(new DelayToSchedulerAvailableStartsOnWhereSpec(Date)),
                                          paginatedSpecification: new PaginatedSpecification(1, FetchSize)));
            all.AddRange(Repository.Query(whereSpecification: (new DelayToSchedulerByStatusWhere(DelayOfStatus.Error))
                                                  .And(new DelayToSchedulerAvailableStartsOnWhereSpec(Date)),
                                          paginatedSpecification: new PaginatedSpecification(1, 10)));
            return all.ToList()
                      .GroupBy(r => r.GroupKey, r => r)
                      .ToDictionary(grouping => grouping.Key, grouping => grouping.ToList());
        }

        #region Properties

        public int FetchSize { get; set; }

        public DateTime Date { get; set; }

        #endregion
    }
}