namespace Incoding.MvcContrib
{
    #region << Using >>

    using Incoding.Extensions;

    #endregion

    public class ExecutableEval : ExecutableBase
    {
        #region Constructors

        public ExecutableEval(string code)
        {
            this.Set("code", code);
        }

        #endregion
    }
}