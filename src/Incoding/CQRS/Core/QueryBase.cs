namespace Incoding.CQRS
{
    public abstract class QueryBase<TResult> : MessageBase<TResult>
    {
        #region Override

        protected override void Execute()
        {
            Result = ExecuteResult();
        }

        #endregion

        protected abstract TResult ExecuteResult();
    }
}