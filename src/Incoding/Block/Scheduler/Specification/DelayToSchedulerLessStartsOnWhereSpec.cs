namespace Incoding.Block
{
    #region << Using >>

    using System;
    using System.Linq.Expressions;

    #endregion

    public class DelayToSchedulerAvailableStartsOnWhereSpec : Specification<DelayToScheduler>
    {
        readonly DateTime date;

        public DelayToSchedulerAvailableStartsOnWhereSpec(DateTime date)
        {
            this.date = date;
        }

        public override Expression<Func<DelayToScheduler, bool>> IsSatisfiedBy()
        {
            var faultAbove = this.date.AddMinutes(2);
            return scheduler => scheduler.StartsOn <= faultAbove;
        }
    }
}