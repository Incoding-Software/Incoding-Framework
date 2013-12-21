namespace Incoding.MvcContrib
{
    #region << Using >>

    using Incoding.Extensions;

    #endregion

    public class ExecutableForm : ExecutableBase
    {
        #region Constructors

        public ExecutableForm(string method)
        {
            this.Set("method", method);
        }

        #endregion
    }
}