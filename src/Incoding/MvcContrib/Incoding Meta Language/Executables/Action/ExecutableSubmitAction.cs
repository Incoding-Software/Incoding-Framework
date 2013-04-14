namespace Incoding.MvcContrib
{
    public class ExecutableSubmitAction : ExecutableActionBase
    {
        #region Constructors

        internal ExecutableSubmitAction(string formSelector, JqueryAjaxFormOptions ajaxForm)
        {
            Data.Add("options", ajaxForm.OptionCollections);
            Data.Add("formSelector", formSelector);
        }

        #endregion
    }
}