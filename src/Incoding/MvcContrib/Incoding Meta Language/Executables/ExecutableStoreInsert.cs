namespace Incoding.MvcContrib
{
    #region << Using >>

    using Incoding.Extensions;

    #endregion

    public class ExecutableStoreInsert : ExecutableBase
    {
        #region Constructors

        public ExecutableStoreInsert(string type, bool replace, string prefix)
        {
            this.Set("type", type);
            this.Set("replace", replace);
            this.Set("prefix", prefix);
        }

        #endregion
    }
}