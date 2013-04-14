namespace Incoding.MvcContrib
{
    #region << Using >>

    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Web;
    using System.Web.Routing;
    using Incoding.Extensions;

    #endregion

    public partial class IncodingMetaLanguageDsl : IIncodingMetaLanguageBindingDsl, IIncodingMetaLanguageEventBuilderDsl, IIncodingMetaLanguageCallbackBodyDsl, IIncodingMetaLanguageCallbackInstancesDsl
    {
        #region Fields

        readonly IncodingMetaContainer meta;

        #endregion

        #region Constructors

        public IncodingMetaLanguageDsl(JqueryBind currentBind)
                : this(currentBind.ToJqueryString()) { }

        public IncodingMetaLanguageDsl(string currentBind)
        {
            this.meta = new IncodingMetaContainer();
            When(currentBind);
        }

        #endregion

        #region IIncodingMetaLanguageBindingDsl Members

        public IIncodingMetaLanguageActionDsl Do()
        {
            this.meta.onEventStatus = IncodingEventCanceled.None;
            return this;
        }

        public IIncodingMetaLanguageActionDsl DoWithPreventDefault()
        {
            this.meta.onEventStatus = IncodingEventCanceled.PreventDefault;
            return this;
        }

        public IIncodingMetaLanguageActionDsl DoWithStopPropagation()
        {
            this.meta.onEventStatus = IncodingEventCanceled.StopPropagation;
            return this;
        }

        public IIncodingMetaLanguageActionDsl DoWithPreventDefaultAndStopPropagation()
        {
            this.meta.onEventStatus = IncodingEventCanceled.All;
            return this;
        }

        #endregion

        #region IIncodingMetaLanguageCallbackBodyDsl Members

        public IIncodingMetaLanguageCallbackInstancesDsl With(JquerySelector selector)
        {
            this.meta.target = selector.ToString();
            return this;
        }

        public IIncodingMetaLanguageCallbackInstancesDsl With(Func<JquerySelector, JquerySelector> action)
        {
            return With(action(Selector.Jquery));
        }

        public IIncodingMetaLanguageCallbackInstancesDsl Self()
        {
            return With(selector => selector.Self());
        }

        #endregion

        #region IIncodingMetaLanguageCallbackInstancesDsl Members

        public IExecutableSetting Registry(ExecutableBase callback)
        {
            this.meta.Add(callback);
            return callback;
        }

        #endregion

        #region IIncodingMetaLanguageEventBuilderDsl Members

        public IIncodingMetaLanguageEventBuilderDsl OnError(Action<IIncodingMetaLanguageCallbackBodyDsl> action)
        {
            return On(IncodingCallbackStatus.Error, action);
        }

        public IIncodingMetaLanguageEventBuilderDsl OnComplete(Action<IIncodingMetaLanguageCallbackBodyDsl> action)
        {
            return On(IncodingCallbackStatus.Complete, action);
        }

        public IIncodingMetaLanguageEventBuilderDsl OnBegin(Action<IIncodingMetaLanguageCallbackBodyDsl> action)
        {
            return On(IncodingCallbackStatus.Begin, action);
        }

        public IIncodingMetaLanguageEventBuilderDsl OnBreak(Action<IIncodingMetaLanguageCallbackBodyDsl> action)
        {
            return On(IncodingCallbackStatus.Break, action);
        }

        public IIncodingMetaLanguageEventBuilderDsl OnSuccess(Action<IIncodingMetaLanguageCallbackBodyDsl> action)
        {
            return On(IncodingCallbackStatus.Success, action);
        }

        public RouteValueDictionary AsHtmlAttributes(object htmlAttributes = null)
        {
            return this.meta.AsHtmlAttributes(htmlAttributes);
        }

        public string AsStringAttributes(object htmlAttributes = null)
        {
            return this.meta.AsHtmlAttributes(htmlAttributes).Aggregate(string.Empty, (s, pair) => s += " {0}=\"{1}\" ".F(pair.Key, HttpUtility.HtmlEncode(pair.Value)));
        }

        public IIncodingMetaLanguageBindingDsl When(JqueryBind nextBind)
        {
            return When(nextBind.ToJqueryString());
        }

        public IIncodingMetaLanguageBindingDsl When(string nextBind)
        {
            this.meta.onBind = nextBind.ToLowerInvariant();

            if (!this.meta.onBind.EqualsWithInvariant("incoding"))
                this.meta.onBind += " incoding";

            return this;
        }

        public IIncodingMetaLanguageEventBuilderDsl Where<TModel>(Expression<Func<TModel, bool>> expression)
        {
            this.meta.SetFilter(new ConditionalData<TModel>(expression, true));
            return this;
        }

        #endregion

        IIncodingMetaLanguageEventBuilderDsl On(IncodingCallbackStatus status, Action<IIncodingMetaLanguageCallbackBodyDsl> action)
        {
            this.meta.onCurrentStatus = status;
            action(this);
            return this;
        }

    }
}