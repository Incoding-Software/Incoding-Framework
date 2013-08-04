namespace Incoding.MvcContrib
{
    #region << Using >>

    using Incoding.Extensions;

    #endregion

    public abstract class ExecutableActionBase : ExecutableBase
    {
        #region Api Methods

        public void SetFilter(ConditionalBase filter)
        {
            Data.Set("filterResult", filter.GetData());
        }

        #endregion
    }
}