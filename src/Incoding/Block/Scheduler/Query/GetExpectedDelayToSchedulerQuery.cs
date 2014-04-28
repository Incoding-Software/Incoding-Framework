namespace Incoding.Block
{
    #region << Using >>

    using System.Collections.Generic;
    using System.Linq;
    using Incoding.CQRS;
    using Incoding.Data;

    #endregion

    public class GetExpectedDelayToSchedulerQuery : QueryBase<Dictionary<string, List<DelayToScheduler>>>
    {
        #region Properties

        public int FetchSize { get; set; }

        #endregion

        protected override Dictionary<string, List<DelayToScheduler>> ExecuteResult()
        {
            return Repository.Query(whereSpecification: !new DelayToSchedulerByStatusWhere(DelayOfStatus.Success),
                                    paginatedSpecification: new PaginatedSpecification(1, FetchSize))
                             .ToList()
                             .GroupBy(r => r.GroupKey, r => r)
                             .ToDictionary(grouping => grouping.Key, grouping => grouping.ToList());
        }
    }
}