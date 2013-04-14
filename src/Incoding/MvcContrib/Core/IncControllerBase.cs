namespace Incoding.MvcContrib
{
    #region << Using >>

    using System;
    using System.IO;
    using System.Web.Mvc;
    using Incoding.CQRS;
    using Incoding.Maybe;
    using JetBrains.Annotations;

    #endregion

    public abstract class IncControllerBase : AsyncController
    {
        #region Fields

        protected readonly IDispatcher dispatcher;

        #endregion

        #region Constructors

        protected IncControllerBase(IDispatcher dispatcher)
        {
            Guard.NotNull("dispatcher", dispatcher);
            this.dispatcher = dispatcher;
        }

        #endregion

        #region Nested classes

        protected class IncTryPushSetting
        {
            #region Properties

            public Func<ActionResult> SuccessResult { get; set; }

            public Func<IncWebException, ActionResult> ErrorResult { get; set; }

            #endregion
        }

        #endregion

        protected IncodingResult IncRedirect(string url)
        {
            return IncodingResult.RedirectTo(url);
        }

        protected IncodingResult IncRedirectToAction([AspMvcAction] string action)
        {
            return IncRedirectToAction(action, null);
        }

        protected IncodingResult IncRedirectToAction([AspMvcAction] string action, [AspMvcController] string controller)
        {
            return IncRedirectToAction(action, controller, null);
        }

        protected IncodingResult IncRedirectToAction([AspMvcAction] string action, [AspMvcController] string controller, object routes)
        {
            return IncodingResult.RedirectTo(Url.Action(action, controller, routes));
        }

        protected IncodingResult IncView(object model = null)
        {
            return IncPartialView(ControllerContext.RouteData.GetRequiredString("action"), model);
        }

        protected IncodingResult IncJson(object model)
        {
            return IncodingResult.Success(model);
        }

        protected IncodingResult IncPartialView([AspMvcView] string viewName, object model = null)
        {
            Guard.NotNullOrWhiteSpace("viewName", viewName);
            ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                return IncodingResult.Success(sw.GetStringBuilder().ToString());
            }
        }

        protected ActionResult TryPush(CommandBase input, Action<IncTryPushSetting> action = null)
        {
            return TryPush(composite => composite.Quote(input), action);
        }

        protected ActionResult TryPush(Action<CommandComposite> configuration, Action<IncTryPushSetting> action = null)
        {
            var composite = new CommandComposite();
            configuration(composite);
            return TryPush(composite, action);
        }

        protected ActionResult TryPush(CommandComposite composite, Action<IncTryPushSetting> action = null)
        {
            var setting = new IncTryPushSetting();
            action.Do(r => r(setting));

            Func<ActionResult> defaultSuccess = () => View(composite.Parts[0].Message);
            if (HttpContext.Request.IsAjaxRequest())
                defaultSuccess = () => IncodingResult.Success();
            var success = setting.SuccessResult ?? defaultSuccess;

            Func<IncWebException, ActionResult> defaultError = (ex) => View(composite.Parts[0].Message);
            if (HttpContext.Request.IsAjaxRequest())
                defaultError = (ex) => IncodingResult.Error(ModelState);
            var error = setting.ErrorResult ?? defaultError;

            if (!ModelState.IsValid)
                return error(IncWebException.Empty);

            try
            {
                this.dispatcher.Push(composite);
                return success();
            }
            catch (IncWebException exception)
            {
                ModelState.AddModelError(exception.Property, exception.Message);
                return error(exception);
            }
        }
    }
}