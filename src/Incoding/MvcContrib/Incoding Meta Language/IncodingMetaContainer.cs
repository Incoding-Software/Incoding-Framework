namespace Incoding.MvcContrib
{
    #region << Using >>

    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Routing;
    using Incoding.Extensions;
    using Incoding.Maybe;

    #endregion

    public class IncodingMetaContainer
    {
        #region Fields

        readonly List<ExecutableBase> merges = new List<ExecutableBase>();

        JquerySelectorExtend target;

        IncodingEventCanceled onEventStatus;

        bool lockTarget;

        #endregion

        #region Constructors

        public IncodingMetaContainer()
        {
            OnEventStatus = IncodingEventCanceled.None;
        }

        #endregion

        #region Properties

        public string OnBind { get; set; }

        public bool LockTarget
        {
            get { return this.lockTarget; }
            set
            {
                if (!value)
                    this.target = null;
                this.lockTarget = value;
            }
        }

        public IncodingEventCanceled OnEventStatus
        {
            get { return this.onEventStatus; }
            set
            {
                bool isAll = (this.onEventStatus == IncodingEventCanceled.PreventDefault && value == IncodingEventCanceled.StopPropagation) ||
                             (this.onEventStatus == IncodingEventCanceled.StopPropagation && value == IncodingEventCanceled.PreventDefault);
                this.onEventStatus = isAll ? IncodingEventCanceled.All : value;
            }
        }

        public JquerySelectorExtend Target
        {
            get { return this.target; }
            set
            {
                this.target = this.target != null
                                      ? this.target.Add(value)
                                      : value;
            }
        }

        public IncodingCallbackStatus OnCurrentStatus { get; set; }

        #endregion

        #region Api Methods

        public RouteValueDictionary AsHtmlAttributes(object htmlAttributes = null)
        {
            var res = AnonymousHelper.ToDictionary(htmlAttributes);
            res = this.merges.Aggregate(res, (current, attributeMerge) => attributeMerge.Merge(current));
            return res;
        }

        public void Add(ExecutableBase callback)
        {
            callback.Add("onBind", OnBind);
            callback.Add("onStatus", (int)OnCurrentStatus);
            callback.Add("target", Target.With(r => r.ToString()));
            callback.Add("onEventStatus", (int)OnEventStatus);
            this.merges.Add(callback);
            if (!LockTarget)
                this.target = null;
        }

        public void SetFilter(ConditionalBase filter)
        {
            this.merges
                .Where(r => r["onBind"].ToString() == OnBind)
                .OfType<ExecutableActionBase>()
                .DoEach(@base => @base.SetFilter(filter));
        }

        #endregion
    }
}