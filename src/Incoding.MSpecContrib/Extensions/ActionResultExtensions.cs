namespace Incoding.MSpecContrib
{
    #region << Using >>

    using System;
    using System.Linq.Expressions;
    using System.Web.Mvc;
    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    public static class ActionResultExtensions
    {
        #region Factory constructors

        public static void ShouldBeFileContent(this ActionResult actionResult, byte[] fileContents, string contentType = "", string fileDownloadName = "")
        {
            var fileResult = actionResult as FileContentResult;

            fileResult.ShouldNotBeNull();
            fileResult.FileContents.ShouldEqualWeakEach(fileContents);
            fileResult.ContentType.ShouldEqual(contentType);
            fileResult.FileDownloadName.ShouldEqual(fileDownloadName);
        }

        public static void ShouldBeIncodingData<TData>(this ActionResult actionResult, Action<TData> verify)
        {
            ShouldBeIncoding(actionResult, data =>
                                               {
                                                   data.data.ShouldBeOfType<TData>();
                                                   verify((TData)data.data);
                                               });
        }

        public static void ShouldBeIncodingData<TData>(this ActionResult actionResult, TData data)
        {
            ShouldBeIncodingData<TData>(actionResult, r => r.ShouldEqualWeak(data));
        }

        public static void ShouldBeIncodingDataIsNull(this ActionResult actionResult)
        {
            ShouldBeIncoding(actionResult, data => data.data.ShouldBeNull());
        }

        public static void ShouldBeIncodingError<TData>(this ActionResult actionResult, Action<TData> verify)
        {
            ShouldBeIncoding(actionResult, data => data.success.ShouldBeFalse());
            ShouldBeIncodingData(actionResult, verify);
        }

        public static void ShouldBeIncodingError(this ActionResult actionResult)
        {
            ShouldBeIncoding(actionResult, data => data.success.ShouldBeFalse());
        }

        public static void ShouldBeIncodingRedirect(this ActionResult actionResult, string redirectTo)
        {
            ShouldBeIncoding(actionResult, data => data.redirectTo.ShouldEqual(redirectTo));
        }

        public static void ShouldBeIncodingSuccess<TData>(this ActionResult actionResult, Action<TData> verify)
        {
            ShouldBeIncoding(actionResult, data => data.success.ShouldBeTrue());
            ShouldBeIncodingData(actionResult, verify);
        }

        public static void ShouldBeIncodingSuccess(this ActionResult actionResult)
        {
            ShouldBeIncoding(actionResult, data => data.success.ShouldBeTrue());
        }

        public static void ShouldBeModel<TModel>(this ActionResult actionResult, Action<TModel> expected)
        {
            var viewResult = actionResult as ViewResult;
            viewResult.ShouldNotBeNull();
            viewResult.Model.ShouldBeOfType<TModel>();
            expected((TModel)viewResult.Model);
        }

        public static void ShouldBeModel<TModel>(this ActionResult actionResult, TModel expected)
        {
            ShouldBeModel<TModel>(actionResult, model => model.ShouldEqualWeak(expected));
        }

        public static void ShouldBeRedirect(this ActionResult actionResult, string url)
        {
            var redirectResult = actionResult as RedirectResult;
            redirectResult.ShouldNotBeNull();
            redirectResult.Url.ShouldEqual(url);
        }

        public static void ShouldBeRedirectToAction<TController>(this ActionResult actionResult, Expression<Action<TController>> action) where TController : Controller
        {
            var redirectResult = actionResult as RedirectToRouteResult;
            redirectResult.ShouldNotBeNull();
            redirectResult.RouteValues["Controller"].ToString().ToLower().ShouldEqual(RoutingName<TController>().ToLower());
            redirectResult.RouteValues["Action"].ToString().ToLower().ShouldEqual(action.GetMethodBodyName().ToLower());
        }

        public static void ShouldBeView(this ActionResult actionResult, string viewName = "")
        {
            (actionResult is ViewResult || actionResult is PartialViewResult).ShouldBeTrue();
            if (actionResult is ViewResult)
                ((ViewResult)actionResult).ViewName.ShouldEqual(viewName);

            if (actionResult is PartialViewResult)
                ((PartialViewResult)actionResult).ViewName.ShouldEqual(viewName);
        }

        public static void ShouldBeView<TModel>(this ActionResult actionResult, TModel model, string viewName = "")
        {
            actionResult.ShouldBeView(viewName);
            actionResult.ShouldBeModel(model);
        }

        #endregion

        static void ShouldBeIncoding(this ActionResult actionResult, Action<IncodingResult.JsonData> verify)
        {
            Guard.NotNull("verify", verify);

            var incodingResult = actionResult as IncodingResult;
            incodingResult.ShouldNotBeNull();

            var defaultJsonData = incodingResult.Data as IncodingResult.JsonData;
            defaultJsonData.ShouldNotBeNull();

            verify(defaultJsonData);
        }

        static string GetMethodBodyName<TController>(this Expression<Action<TController>> action) where TController : Controller
        {
            action.ShouldNotBeNull();
            action.Body.ShouldBeOfType<MethodCallExpression>();
            return ((MethodCallExpression)action.Body).Method.Name;
        }

        static string RoutingName<TController>() where TController : Controller
        {
            string controllerName = typeof(TController).Name;
            controllerName.EndsWith("Controller", StringComparison.OrdinalIgnoreCase).ShouldBeTrue();

            controllerName = controllerName.Substring(0, controllerName.Length - "Controller".Length);

            controllerName.ShouldNotBeEmpty();

            return controllerName;
        }
    }
}