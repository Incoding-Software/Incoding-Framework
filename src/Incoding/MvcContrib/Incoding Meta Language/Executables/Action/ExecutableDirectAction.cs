namespace Incoding.MvcContrib
{
    public class ExecutableDirectAction : ExecutableActionBase
    {
        #region Constructors

        public ExecutableDirectAction(string result)
        {
            this["result"] = result;
        }

        #endregion
    }
}