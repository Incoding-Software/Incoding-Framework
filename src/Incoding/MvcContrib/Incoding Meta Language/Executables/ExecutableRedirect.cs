namespace Incoding.MvcContrib
{
    #region << Using >>

    using Incoding.Extensions;

    #endregion

    public class ExecutableRedirect : ExecutableBase
    {
        #region Constructors

        public ExecutableRedirect(string redirectTo)
        {
            if (!string.IsNullOrWhiteSpace(redirectTo))
                Data.Set("redirectTo", redirectTo);
        }

        #endregion
    }
}