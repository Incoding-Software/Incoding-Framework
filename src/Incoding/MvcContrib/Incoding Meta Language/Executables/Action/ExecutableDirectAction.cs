namespace Incoding.MvcContrib
{
    public class ExecutableDirectAction : ExecutableActionBase
    {
        #region Constructors

        public ExecutableDirectAction(string result)
        {
            Data["result"] = result;
        }

        #endregion
    }
}