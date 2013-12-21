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

        public string onBind;

        public IncodingEventCanceled onEventStatus;

        public string target;

        public IncodingCallbackStatus onCurrentStatus;

        readonly List<ExecutableBase> merges = new List<ExecutableBase>();

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
            callback.Add("onBind", this.onBind);
            callback.Add("onStatus", (int)this.onCurrentStatus);
            callback.Add("target", this.target);
            callback.Add("onEventStatus", (int)this.onEventStatus);
            this.merges.Add(callback);
        }

        public void SetFilter(ConditionalBase filter)
        {
            this.merges
                .Where(r => r["onBind"].ToString() == this.onBind)
                .OfType<ExecutableActionBase>()
                .DoEach(@base => @base.SetFilter(filter));
        }

        #endregion
    }
}