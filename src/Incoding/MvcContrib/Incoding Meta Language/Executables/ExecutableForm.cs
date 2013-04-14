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
            Data.Set("method", method);
        }

        #endregion
    }
}