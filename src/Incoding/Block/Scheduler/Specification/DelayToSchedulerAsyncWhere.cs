namespace Incoding.Block
{
    using System;
    using System.Linq.Expressions;

    public class DelayToSchedulerAsyncWhere : Specification<DelayToScheduler>
    {
        private readonly bool @async;

        public DelayToSchedulerAsyncWhere(bool @async)
        {
            this.async = async;
        }

        public override Expression<Func<DelayToScheduler, bool>> IsSatisfiedBy()
        {
            return scheduler => scheduler.Option.Async == this.async;
        }
    }
}