namespace Incoding.MvcContrib
{
    public class ExecutableSubmitAction : ExecutableActionBase
    {
        #region Constructors

        public ExecutableSubmitAction(string formSelector, JqueryAjaxFormOptions ajaxForm)
        {            
            this.Add("formSelector", formSelector);
            this.Add("options", ajaxForm);
        }

        #endregion
    }
}