namespace Incoding.MvcContrib
{
    #region << Using >>

    using System;
    using System.Web.Mvc;
    using JetBrains.Annotations;

    #endregion

    public static class ViewContextExtensions
    {
        #region Factory constructors

        public static bool IsAction(this ViewContext context, [AspMvcAction] string action)
        {
            string currentAction = context.TryGetRouteData("action");
            return currentAction.Equals(action, StringComparison.InvariantCultureIgnoreCase);
        }

        public static bool IsArea(this ViewContext context, [AspMvcArea] string area)
        {
            string currentArea = context.TryGetRouteData("area");
            return currentArea.Equals(area, StringComparison.InvariantCultureIgnoreCase);
        }

        public static bool IsController(this ViewContext context, [AspMvcController] string controller)
        {
            string currentController = context.TryGetRouteData("controller");
            return currentController.Equals(controller, StringComparison.InvariantCultureIgnoreCase);
        }

        public static bool IsCurrent(this ViewContext context, [AspMvcAction] string action, [AspMvcController] string controller, [AspMvcArea] string area = "")
        {
            bool res = context.IsController(controller) && context.IsAction(action);
            if (!string.IsNullOrWhiteSpace(area))
                res = context.IsArea(area);

            return res;
        }

        #endregion

        static string TryGetRouteData(this ViewContext context, string key)
        {
            return context.RouteData.DataTokens[key] != null
                           ? context.RouteData.DataTokens[key].ToString()
                           : string.Empty;
        }
    }
}