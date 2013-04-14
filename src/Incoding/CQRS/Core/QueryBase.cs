namespace Incoding.CQRS
{
    public abstract class QueryBase<TResult> : MessageBase<TResult> where TResult : class
    {
        #region Override

        public override void Execute()
        {
            Result = ExecuteResult();
        }

        #endregion

        protected abstract TResult ExecuteResult();
    }
}