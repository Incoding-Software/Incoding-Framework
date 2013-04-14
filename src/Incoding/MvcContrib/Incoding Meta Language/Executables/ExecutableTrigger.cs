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
            Data.Set("trigger", trigger);
            if (!string.IsNullOrWhiteSpace(property))
                Data.Set("property", property);
        }

        #endregion
    }
}