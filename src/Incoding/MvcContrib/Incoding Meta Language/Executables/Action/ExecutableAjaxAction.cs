namespace Incoding.MvcContrib
{
    public class ExecutableAjaxAction : ExecutableActionBase
    {
        #region Constructors

        public ExecutableAjaxAction(bool hash, string prefix, JqueryAjaxOptions ajax)
        {            
            this["ajax"] = ajax;
            this["hash"] = hash;
            this["prefix"] = prefix;
        }

        #endregion
    }
}