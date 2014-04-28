namespace Incoding.Block
{
    using System;
    using System.Linq.Expressions;

    public class DelayToSchedulerByUIDWhereSpec : Specification<DelayToScheduler>
    {
        #region Fields

        readonly string uid;

        #endregion

        #region Constructors

        public DelayToSchedulerByUIDWhereSpec(string uid)
        {
            this.uid = uid;
        }

        #endregion

        public override Expression<Func<DelayToScheduler, bool>> IsSatisfiedBy()
        {
            return scheduler => scheduler.UID == this.uid;
        }
    }
}