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
            this.Set("type", type);
            if (!string.IsNullOrWhiteSpace(meta))
                this.Set("meta", meta);
            if (!string.IsNullOrWhiteSpace(bind))
                this.Set("bind", bind);
        }

        #endregion
    }
}