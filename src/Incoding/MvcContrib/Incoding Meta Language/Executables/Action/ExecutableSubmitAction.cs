namespace Incoding.MvcContrib
{
    public class ExecutableSubmitAction : ExecutableActionBase
    {
        #region Constructors

        public ExecutableSubmitAction(string formSelector, JqueryAjaxFormOptions ajaxForm)
        {
            this.Add("options", ajaxForm);
            this.Add("formSelector", formSelector);
        }

        #endregion
    }
}