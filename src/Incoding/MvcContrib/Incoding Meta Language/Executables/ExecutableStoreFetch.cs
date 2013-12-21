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
            this.Set("type", type);
            this.Set("prefix", prefix);
        }

        #endregion
    }
}