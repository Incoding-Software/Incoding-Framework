namespace Incoding.MvcContrib
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Web;
    using System.Web.Routing;
    using Incoding.Extensions;

    #endregion

    public partial class IncodingMetaLanguageDsl : IncodingMetaLanguageCoreDsl,IIncodingMetaLanguageBindingDsl, IIncodingMetaLanguageEventBuilderDsl, IIncodingMetaLanguageCallbackBodyDsl, IIncodingMetaLanguageCallbackInstancesDsl
    {
        #region Fields

        readonly IncodingMetaContainer meta;

        #endregion

        #region Constructors

        public IncodingMetaLanguageDsl(JqueryBind currentBind)
                : this(currentBind.ToJqueryString()) { }

        public IncodingMetaLanguageDsl(string currentBind)
            :base(null)
        {
            this.plugIn = this;
            this.meta = new IncodingMetaContainer();
            When(currentBind);
        }

        #endregion

        public override string ToString()
        {
            throw new NotImplementedException("Please finishing IML expression with AsHtmlAttributes().ToTag(some)");
        }

        #region IIncodingMetaLanguageBindingDsl Members

        public IIncodingMetaLanguageActionDsl Do()
        {
            this.meta.OnEventStatus = IncodingEventCanceled.None;
            return this;
        }

        public IIncodingMetaLanguageActionDsl PreventDefault()
        {
            this.meta.OnEventStatus = this.meta.OnEventStatus == IncodingEventCanceled.StopPropagation ? IncodingEventCanceled.All : IncodingEventCanceled.PreventDefault;
            return this;
        }

        public IIncodingMetaLanguageActionDsl StopPropagation()
        {
            this.meta.OnEventStatus = this.meta.OnEventStatus == IncodingEventCanceled.PreventDefault ? IncodingEventCanceled.All : IncodingEventCanceled.StopPropagation;
            return this;
        }

        public IIncodingMetaLanguageActionDsl DoWithPreventDefault()
        {
            this.meta.OnEventStatus = IncodingEventCanceled.PreventDefault;
            return this;
        }

        public IIncodingMetaLanguageActionDsl DoWithStopPropagation()
        {
            this.meta.OnEventStatus = IncodingEventCanceled.StopPropagation;
            return this;
        }

        public IIncodingMetaLanguageActionDsl DoWithPreventDefaultAndStopPropagation()
        {
            this.meta.OnEventStatus = IncodingEventCanceled.All;
            return this;
        }

        #endregion

        #region IIncodingMetaLanguageCallbackBodyDsl Members

        public IIncodingMetaLanguageCallbackInstancesDsl With(JquerySelectorExtend selector)
        {
            this.meta.Target = selector;
            return this;
        }

        public IIncodingMetaLanguageCallbackInstancesDsl With(Func<JquerySelector, JquerySelectorExtend> action)
        {
            return With(action(Selector.Jquery));
        }

        public IIncodingMetaLanguageCallbackInstancesDsl WithName<T>(Expression<Func<T, object>> memberName)
        {
            return With(selector => selector.Name(memberName));
        }

        public IIncodingMetaLanguageCallbackInstancesDsl WithName(string name)
        {
            return With(selector => selector.Name(name));
        }

        public IIncodingMetaLanguageCallbackInstancesDsl WithId<T>(Expression<Func<T, object>> memberId)
        {
            return With(selector => selector.Id(memberId));
        }

        public IIncodingMetaLanguageCallbackInstancesDsl WithId(string id)
        {
            return With(selector => selector.Id(id));
        }

        public IIncodingMetaLanguageCallbackInstancesDsl WithClass(string @class)
        {
            return With(selector => selector.Class(@class));
        }

        public IIncodingMetaLanguageCallbackInstancesDsl WithClass(B @class)
        {
            return With(selector => selector.Class(@class));
        }

        public IIncodingMetaLanguageCallbackInstancesDsl WithSelf(Func<JquerySelectorExtend, JquerySelectorExtend> self)
        {
            var selector = Selector.Jquery.Self();            
            return With(self(selector));
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

        public void Lock()
        {
            this.meta.LockTarget = true;
        }

        public void UnLock()
        {
            this.meta.LockTarget = false;
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
            this.meta.OnBind = nextBind.ToLowerInvariant();

            if (!this.meta.OnBind.EqualsWithInvariant("incoding"))
                this.meta.OnBind += " incoding";

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
            this.meta.OnCurrentStatus = status;
            action(this);
            return this;
        }
    }
}