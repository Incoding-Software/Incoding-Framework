namespace Incoding.MvcContrib
{
    #region << Using >>

    using Incoding.Extensions;

    #endregion

    public class ExecutableValidationRefresh : ExecutableBase
    {
        public ExecutableValidationRefresh(string prefix)
        {
            if (!string.IsNullOrWhiteSpace(prefix))
                this.Set("prefix", prefix);
        }
    }
}