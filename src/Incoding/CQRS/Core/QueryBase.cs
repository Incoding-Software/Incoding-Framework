namespace Incoding.CQRS
{
    using System.Collections.Generic;
    using Incoding.Block;

    public abstract class QueryBase<TResult> : MessageBase
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