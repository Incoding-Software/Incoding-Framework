namespace Incoding.Block
{
    using System;
    using System.Linq.Expressions;

    public class DelayToSchedulerAvaialbeStartsOnWhereSpec : Specification<DelayToScheduler>
    {
        readonly DateTime date;

        public DelayToSchedulerAvaialbeStartsOnWhereSpec(DateTime date)
        {
            this.date = date;
        }

        public override Expression<Func<DelayToScheduler, bool>> IsSatisfiedBy()
        {
            var faultBelowe = this.date.AddMinutes(-2);
            var faultAbove = this.date.AddMinutes(2);
            return scheduler => scheduler.StartsOn >= faultBelowe ||
                                scheduler.StartsOn <= faultAbove;
        }
    }
}