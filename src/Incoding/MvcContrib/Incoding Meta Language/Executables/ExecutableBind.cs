namespace Incoding.MvcContrib
{
    #region << Using >>

    using Incoding.Extensions;

    #endregion

    public class ExecutableBind : ExecutableBase
    {
        #region Constructors

        public ExecutableBind(string type, string meta, string bind)
        {
            Data.Set("type", type);
            if (!string.IsNullOrWhiteSpace(meta))
                Data.Set("meta", meta);
            if (!string.IsNullOrWhiteSpace(bind))
                Data.Set("bind", bind);
        }

        #endregion
    }
}