namespace Incoding.MvcContrib
{
    #region << Using >>

    using Incoding.Extensions;

    #endregion

    public class ExecutableStoreFetch : ExecutableBase
    {
        #region Constructors

        public ExecutableStoreFetch(string type, string prefix)
        {
            Data.Set("type", type);
            Data.Set("prefix", prefix);
        }

        #endregion
    }
}