namespace Incoding.MvcContrib
{
    public class ExecutableAjaxAction : ExecutableActionBase
    {
        #region Constructors

        internal ExecutableAjaxAction(bool hash, string prefix, JqueryAjaxOptions ajax)
        {
            Data["ajax"] = ajax.OptionCollections;
            Data["hash"] = hash;
            Data["prefix"] = prefix;
        }

        #endregion
    }
}