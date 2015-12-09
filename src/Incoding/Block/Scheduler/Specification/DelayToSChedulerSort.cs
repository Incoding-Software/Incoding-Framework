namespace Incoding.Block
{
    using System;
    using Incoding.Data;

    public class DelayToSchedulerSort : OrderSpecification<DelayToScheduler>
    {
        public override Action<AdHocOrderSpecification<DelayToScheduler>> SortedBy()
        {
            return specification => specification.OrderByDescending(r => r.Priority)
                                                 .OrderByDescending(r => r.StartsOn);
        }
    }
}