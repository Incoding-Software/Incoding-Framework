namespace Incoding.MvcContrib
{
    #region << Using >>

    using Incoding.Extensions;

    #endregion

    public class ExecutableStoreInsert : ExecutableBase
    {
        #region Constructors

        internal ExecutableStoreInsert(string type, bool replace, string prefix)
        {
            Data.Set("type", type);
            Data.Set("replace", replace);
            Data.Set("prefix", prefix);
        }

        #endregion
    }
}