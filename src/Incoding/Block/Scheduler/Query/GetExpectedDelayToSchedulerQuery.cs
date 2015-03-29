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
        #region Properties

        public int FetchSize { get; set; }

        public DateTime Date { get; set; }

        #endregion

        protected override Dictionary<string, List<DelayToScheduler>> ExecuteResult()
        {
            return Repository.Query(whereSpecification: !new DelayToSchedulerByStatusWhere(DelayOfStatus.Success)
                                                                 .And(new DelayToSchedulerAvaialbeStartsOnWhereSpec(Date)),
                                    paginatedSpecification: new PaginatedSpecification(1, FetchSize))
                             .ToList()
                             .GroupBy(r => r.GroupKey, r => r)
                             .ToDictionary(grouping => grouping.Key, grouping => grouping.ToList());
        }
    }
}