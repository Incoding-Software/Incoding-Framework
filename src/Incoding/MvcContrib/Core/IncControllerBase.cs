namespace Incoding.MvcContrib
{
    #region << Using >>

    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Web.Mvc;
    using Incoding.Block.IoC;
    using Incoding.CQRS;
    using Incoding.Maybe;
    using JetBrains.Annotations;

    #endregion

    // ReSharper disable PublicConstructorInAbstractClass
    public abstract class IncControllerBase : Controller
    {
        #region Fields

        readonly Lazy<IDispatcher> _dispatcher;

        #endregion

        #region Properties

        protected IDispatcher dispatcher { get { return this._dispatcher.Value; } }

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

        [AspMvcView]
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
            return IncodingResult.Success(RenderToString(viewName, model));
        }

        protected string RenderToString([AspMvcView] string viewName, object model)
        {
            Guard.NotNullOrWhiteSpace("viewName", viewName);
            ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                return sw.GetStringBuilder().ToString();
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
            return TryPush(commandComposite => dispatcher.Push(commandComposite), composite, action);
        }

        protected ActionResult TryPush(Action<CommandComposite> push, CommandComposite composite, Action<IncTryPushSetting> action = null, bool? isAjax = null)
        {
            var setting = new IncTryPushSetting();
            action.Do(r => r(setting));

            Func<ActionResult> defaultSuccess = () => View(composite.Parts[0]);
            var isActualAjax = isAjax.GetValueOrDefault(HttpContext.Request.IsAjaxRequest());
            if (isActualAjax)
                defaultSuccess = () => IncodingResult.Success();
            var success = setting.SuccessResult ?? defaultSuccess;

            Func<IncWebException, ActionResult> defaultError = (ex) => View(composite.Parts[0]);
            if (isActualAjax)
                defaultError = (ex) => IncodingResult.Error(ModelState);
            var error = setting.ErrorResult ?? defaultError;

            if (!ModelState.IsValid)
                return error(IncWebException.For(string.Empty, string.Empty));

            try
            {
                push(composite);
                return success();
            }
            catch (IncWebException exception)
            {
                foreach (var pairError in exception.Errors)
                {
                    foreach (var errorMessage in pairError.Value)
                        ModelState.AddModelError(pairError.Key, errorMessage);
                }

                return error(exception);
            }
        }

        #region Nested classes

        protected class IncTryPushSetting
        {
            #region Properties

            public Func<ActionResult> SuccessResult { get; set; }

            public Func<IncWebException, ActionResult> ErrorResult { get; set; }

            #endregion
        }

        #endregion

        #region Constructors

        [UsedImplicitly, Obsolete("Please use default ctor without parameters", false), ExcludeFromCodeCoverage]
        public IncControllerBase(IDispatcher dispatcher)
        {
            ////ncrunch: no coverage start
            this._dispatcher = new Lazy<IDispatcher>(() => dispatcher);
            ////ncrunch: no coverage end        
        }

        public IncControllerBase()
        {
            this._dispatcher = new Lazy<IDispatcher>(() => IoCFactory.Instance.TryResolve<IDispatcher>());
        }

        #endregion
    }
}