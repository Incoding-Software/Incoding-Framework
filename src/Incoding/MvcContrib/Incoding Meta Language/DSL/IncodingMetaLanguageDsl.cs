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

    public partial class IncodingMetaLanguageDsl : IncodingMetaLanguageCoreDsl, IIncodingMetaLanguageBindingDsl, IIncodingMetaLanguageCallbackBodyDsl, IIncodingMetaLanguageCallbackInstancesDsl, IIncodingMetaLanguageSettingEventDsl
    {
        public override string ToString()
        {
            throw new NotImplementedException("Please finishing IML expression with AsHtmlAttributes().ToTag(some)");
        }

        IIncodingMetaLanguageEventBuilderDsl On(IncodingCallbackStatus status, Action<IIncodingMetaLanguageCallbackBodyDsl> action)
        {
            if (this.isEmptyAction)
                meta.Add(new ExecutableDirectAction(string.Empty));

            this.isEmptyAction = false;
            meta.OnCurrentStatus = status;
            action(this);
            return this;
        }

        #region Fields

        readonly IncodingMetaContainer meta;

        bool isEmptyAction;

        #endregion

        #region Constructors

        public IncodingMetaLanguageDsl(JqueryBind currentBind)
                : this(currentBind.ToJqueryString()) { }

        public IncodingMetaLanguageDsl(string currentBind)
                : base(null)
        {
            plugIn = this;
            meta = new IncodingMetaContainer();
            When(currentBind);
        }

        #endregion

        #region IIncodingMetaLanguageBindingDsl Members

        public IIncodingMetaLanguageActionDsl Do()
        {
            meta.OnEventStatus = IncodingEventCanceled.None;
            return this;
        }

        public IIncodingMetaLanguageSettingEventDsl PreventDefault()
        {
            meta.OnEventStatus = meta.OnEventStatus == IncodingEventCanceled.StopPropagation ? IncodingEventCanceled.All : IncodingEventCanceled.PreventDefault;
            return this;
        }

        public IIncodingMetaLanguageSettingEventDsl StopPropagation()
        {
            meta.OnEventStatus = meta.OnEventStatus == IncodingEventCanceled.PreventDefault ? IncodingEventCanceled.All : IncodingEventCanceled.StopPropagation;
            return this;
        }

        public IIncodingMetaLanguageActionDsl DoWithPreventDefault()
        {
            meta.OnEventStatus = IncodingEventCanceled.PreventDefault;
            return this;
        }

        public IIncodingMetaLanguageActionDsl DoWithStopPropagation()
        {
            meta.OnEventStatus = IncodingEventCanceled.StopPropagation;
            return this;
        }

        public IIncodingMetaLanguageActionDsl DoWithPreventDefaultAndStopPropagation()
        {
            meta.OnEventStatus = IncodingEventCanceled.All;
            return this;
        }

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
            return meta.AsHtmlAttributes(htmlAttributes);
        }

        public RouteValueDictionary AsHtmlAttributes(string id = "", string classes = "", bool disabled = false, bool readOnly = false,
                                                     bool autocomplete = false, string placeholder = "", string title = "")
        {
            var routes = new RouteValueDictionary();
            if (!string.IsNullOrWhiteSpace(id))
                routes.Add(HtmlAttribute.Id.ToStringLower(), id);
            if (!string.IsNullOrWhiteSpace(placeholder))
                routes.Add(HtmlAttribute.Placeholder.ToStringLower(), placeholder);
            if (!string.IsNullOrWhiteSpace(title))
                routes.Add(HtmlAttribute.Placeholder.ToStringLower(), title);
            if (!string.IsNullOrWhiteSpace(classes))
                routes.Add(HtmlAttribute.Class.ToStringLower(), classes);
            if (disabled)
                routes.Add(HtmlAttribute.Disabled.ToStringLower(), HtmlAttribute.Disabled.ToStringLower());
            if (readOnly)
                routes.Add(HtmlAttribute.Readonly.ToStringLower(), HtmlAttribute.Readonly.ToStringLower());
            if (autocomplete)
                routes.Add(HtmlAttribute.AutoComplete.ToStringLower(), HtmlAttribute.AutoComplete.ToStringLower());

            return meta.AsHtmlAttributes(routes);
        }

        public string AsStringAttributes(object htmlAttributes = null)
        {
            return meta.AsHtmlAttributes(htmlAttributes).Aggregate(string.Empty, (s, pair) => s += " {0}=\"{1}\" ".F(pair.Key, HttpUtility.HtmlEncode(pair.Value)));
        }

        public IIncodingMetaLanguageBindingDsl When(JqueryBind nextBind)
        {
            return When(nextBind.ToJqueryString());
        }

        public IIncodingMetaLanguageBindingDsl When(string nextBind)
        {
            this.isEmptyAction = true;
            meta.OnEventStatus = IncodingEventCanceled.None;
            meta.OnBind = nextBind.ToLowerInvariant();

            if (!meta.OnBind.EqualsWithInvariant("incoding"))
                meta.OnBind += " incoding";

            return this;
        }

        public IIncodingMetaLanguageEventBuilderDsl Where<TModel>(Expression<Func<TModel, bool>> expression)
        {
            meta.SetFilter(new ConditionalData<TModel>(expression, true));
            return this;
        }

        #endregion

        #region IIncodingMetaLanguageCallbackBodyDsl Members

        public IIncodingMetaLanguageCallbackInstancesDsl With(JquerySelectorExtend selector)
        {
            meta.Target = selector;
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
            meta.Add(callback);
            return callback;
        }

        public void Lock()
        {
            meta.LockTarget = true;
        }

        public void UnLock()
        {
            meta.LockTarget = false;
        }

        #endregion
    }
}