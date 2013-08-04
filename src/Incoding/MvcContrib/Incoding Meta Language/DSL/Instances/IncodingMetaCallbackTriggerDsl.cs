namespace Incoding.MvcContrib
{
    #region << Using >>

    using System;
    using System.Linq.Expressions;
    using Incoding.Extensions;

    #endregion

    public class IncodingMetaCallbackTriggerDsl
    {
        #region Fields

        readonly IIncodingMetaLanguagePlugInDsl plugIn;

        string triggerProperty = string.Empty;

        #endregion

        #region Constructors

        public IncodingMetaCallbackTriggerDsl(IIncodingMetaLanguagePlugInDsl plugIn)
        {
            this.plugIn = plugIn;
        }

        #endregion

        #region Api Methods

        public IExecutableSetting Invoke(JqueryBind trigger)
        {
            return Invoke(FixBindsAsString(trigger));
        }

        public IExecutableSetting Invoke(string trigger)
        {
            return this.plugIn.Registry(new ExecutableTrigger(trigger.ToLower(), this.triggerProperty));
        }

        public IExecutableSetting Incoding()
        {
            return Invoke(JqueryBind.Incoding);
        }

        public IncodingMetaCallbackTriggerDsl For<TModel>(Expression<Func<TModel, object>> property)
        {
            this.triggerProperty = property.GetMemberName();
            return this;
        }

        #endregion

        protected static string FixBindsAsString(JqueryBind bind)
        {
            return bind
                    .ToString().Replace(",", string.Empty)
                    .ToLowerInvariant();
        }
    }
}