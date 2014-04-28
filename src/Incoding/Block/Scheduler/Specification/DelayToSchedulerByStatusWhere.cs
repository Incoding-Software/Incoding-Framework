namespace Incoding.Block
{
    using System;
    using System.Linq.Expressions;

    public class DelayToSchedulerByStatusWhere : Specification<DelayToScheduler>
    {
        #region Fields

        readonly DelayOfStatus status;

        #endregion

        #region Constructors

        public DelayToSchedulerByStatusWhere(DelayOfStatus status)
        {
            this.status = status;
        }

        #endregion

        public override Expression<Func<DelayToScheduler, bool>> IsSatisfiedBy()
        {
            return scheduler => scheduler.Status == this.status;
        }
    }
}