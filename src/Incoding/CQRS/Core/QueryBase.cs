namespace Incoding.CQRS
{
    public abstract class QueryBase<TResult> : MessageBase<TResult>
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