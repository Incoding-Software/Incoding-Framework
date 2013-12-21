namespace Incoding.MvcContrib
{
    #region << Using >>

    using Incoding.Extensions;

    #endregion

    public class ExecutableTrigger : ExecutableBase
    {
        #region Constructors

        public ExecutableTrigger(string trigger, string property)
        {
            this.Set("trigger", trigger);
            if (!string.IsNullOrWhiteSpace(property))
                this.Set("property", property);
        }

        #endregion
    }
}