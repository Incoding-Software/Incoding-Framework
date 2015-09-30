namespace Incoding.MSpecContrib
{
    #region << Using >>

    using System;
    using System.Collections.Specialized;
    using System.IO;
    using System.Linq;
    using System.Security.Principal;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Incoding.Block.IoC;
    using Incoding.CQRS;
    using Incoding.Extensions;
    using Incoding.MvcContrib;
    using Moq;

    #endregion

    /// <summary>
    /// Wrapping MVC Controller and Mock everything inside
    /// </summary>
    /// <typeparam name="TController">MVC Controller</typeparam>
    public class MockController<TController> where TController : IncControllerBase
    {
        #region Fields
        // mocking IDispatcher interface
        readonly Mock<IDispatcher> dispatcher;
        // storing original controller to call it's functions for actual code execution
        readonly TController originalController;
        // mocking all HttpContext calls
        readonly Mock<HttpContextBase> httpContext;
        // mocking IView calls
        Mock<IView> view;
        // mocking IViewEngine calls
        Mock<IViewEngine> viewEngine;

        #endregion

        #region Constructors
        /// <summary>
        /// 
        /// </summary>
        /// <param name="controller">original controller</param>
        /// <param name="dispatcher">mock of IDispatcher</param>
        MockController(TController controller, Mock<IDispatcher> dispatcher)
        {
            this.dispatcher = dispatcher;
            this.originalController = controller;

            this.httpContext = Pleasure.Mock<HttpContextBase>();

            // preparing RouteData for ControllerContext
            var routeData = new RouteData();
            routeData.Values.Add("Action", "Action");
            routeData.Values.Add("Controller", "Controller");

            // replacing original controller Context with Mock objects
            this.originalController.ControllerContext = new ControllerContext(this.httpContext.Object, routeData, Pleasure.Mock<ControllerBase>().Object);

            // preparing routes for UrlHelper
            var routes = new RouteCollection();
            routes.MapRoute(name: "Default", 
                            url: "{controller}/{action}/{id}", 
                            defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional });

            // replacing original controller UrlHelper with Mock objects
            this.originalController.Url = new UrlHelper(new RequestContext(this.httpContext.Object, routeData), routes);

            // replace MVC ViewEngines with Mock objects
            AddViewEngine();
        }

        #endregion

        #region Factory constructors
        /// <summary>
        /// Static method to construct MockController object
        /// </summary>
        /// <param name="ctorArgs">original controller constructor parameters</param>
        /// <returns></returns>
        public static MockController<TController> When(params object[] ctorArgs)
        {
            var dispatcher = Pleasure.Mock<IDispatcher>();
            // Return Mock when call to IoC helper IoCFactory.Instance.TryResolve()
            IoCFactory.Instance.StubTryResolve(dispatcher.Object);

            var controller = (TController)Activator.CreateInstance(typeof(TController), ctorArgs.ToArray());
            var res = new MockController<TController>(controller, dispatcher);
            res.httpContext.SetupGet(r => r.Request.Headers).Returns(new NameValueCollection { { "X-Requested-With", "XMLHttpRequest" } });

            return res; 
        }

        #endregion

        #region Properties

        // return original controller
        public TController Original { get { return this.originalController; } }

        #endregion

        #region Api Methods

        /// <summary>
        /// Disabling Ajax (no headers setup)
        /// </summary>
        /// <returns></returns>
        public MockController<TController> DisableAjax()
        {
            return SetupHttpContext(mock => mock.SetupGet(r => r.Request.Headers).Returns(new NameValueCollection()));
        }

        /// <summary>
        /// Mocking Request.Url calls
        /// </summary>
        /// <param name="requestUri">url</param>
        /// <returns></returns>
        public MockController<TController> StubRequestUrl(Uri requestUri)
        {
            return SetupHttpContext(mock =>
                                        {
                                            mock.SetupGet(r => r.Request.Url).Returns(requestUri);
                                            mock.SetupGet(r => r.Request.UrlReferrer).Returns(requestUri);
                                        });
        }

        /// <summary>
        /// Configuring HttpContextBase outside
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public MockController<TController> SetupHttpContext(Action<Mock<HttpContextBase>> action)
        {
            action(this.httpContext);
            return this;
        }

        /// <summary>
        /// Mock Url.Action calls
        /// </summary>
        /// <param name="expectedRoute">expected route url</param>
        /// <returns></returns>
        public MockController<TController> StubUrlAction(string expectedRoute)
        {
            return SetupHttpContext(mock =>
                                        {
                                            mock.Setup(r => r.Response.ApplyAppPathModifier(Pleasure.MockIt.IsStrong(expectedRoute))).Returns(expectedRoute);
                                            mock.Setup(r => r.Request.ApplicationPath).Returns("/");
                                        });
        }

        /// <summary>
        /// Mock Url.Action calls
        /// </summary>
        /// <param name="verifyRoutes">action to verify route url</param>
        /// <param name="expectedRoute">expected route url</param>
        /// <returns></returns>
        public MockController<TController> StubUrlAction(Action<string> verifyRoutes, string expectedRoute)
        {
            return SetupHttpContext(mock =>
                                        {
                                            mock.Setup(r => r.Response.ApplyAppPathModifier(Pleasure.MockIt.Is(verifyRoutes))).Returns(expectedRoute);
                                            mock.Setup(r => r.Request.ApplicationPath).Returns("/");
                                        });
        }

        /// <summary>
        /// Mock calls to QueryBase derived classes and return own data
        /// </summary>
        /// <typeparam name="TQuery"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="query"></param>
        /// <param name="result"></param>
        /// <param name="executeSetting"></param>
        /// <returns></returns>
        public MockController<TController> StubQuery<TQuery, TResult>(TQuery query, TResult result, MessageExecuteSetting executeSetting = null) where TQuery : QueryBase<TResult> where TResult : class
        {
            this.dispatcher.StubQuery(query, result, executeSetting);
            return this;
        }

        /// <summary>
        /// Mock HttpContext.User object
        /// </summary>
        /// <param name="principal"></param>
        /// <returns></returns>
        public MockController<TController> StubPrincipal(IPrincipal principal)
        {
            return SetupHttpContext(mock => mock.Setup(r => r.User).Returns(principal));
        }

        /// <summary>
        /// Mock HttpContext.QueryString object
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public MockController<TController> StubQueryString(object values)
        {
            var nameValueCollection = new NameValueCollection();
            foreach (var keyValuePair in AnonymousHelper.ToDictionary(values))
                nameValueCollection.Add(keyValuePair.Key, keyValuePair.Value.ToString());

            this
                    .httpContext
                    .SetupGet(r => r.Request.QueryString)
                    .Returns(nameValueCollection);

            return this;
        }

        /// <summary>
        /// Throw an exception for Dispatcher.Push call
        /// </summary>
        /// <typeparam name="TCommand"></typeparam>
        /// <param name="command"></param>
        /// <param name="exception"></param>
        /// <param name="executeSetting"></param>
        /// <returns></returns>
        public MockController<TController> StubPushAsThrow<TCommand>(TCommand command, Exception exception, MessageExecuteSetting executeSetting = null) where TCommand : CommandBase
        {
            this.dispatcher.StubPushAsThrow(command, exception, executeSetting);
            return this;
        }

        /// <summary>
        /// Throw an exception for Dispatcher.Query call
        /// </summary>
        /// <typeparam name="TQuery"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="query"></param>
        /// <param name="exception"></param>
        /// <returns></returns>
        public MockController<TController> StubQueryAsThrow<TQuery, TResult>(TQuery query, Exception exception) where TQuery : QueryBase<TResult> where TResult : class
        {
            this.dispatcher.StubQueryAsThrow<TQuery, TResult>(query, exception);
            return this;
        }

        /// <summary>
        /// Assert View is rendered
        /// </summary>
        /// <param name="viewName"></param>
        public void ShouldBeRenderView(string viewName = "Action")
        {
            ShouldBeRenderModel(null, viewName);
        }

        /// <summary>
        /// Assert Model on View is rendered
        /// </summary>
        /// <param name="model"></param>
        /// <param name="viewName"></param>
        public void ShouldBeRenderModel(object model, string viewName = "Action")
        {
            ShouldBeRenderModel<object>(o => o.ShouldEqualWeak(model), viewName);
        }

        /// <summary>
        /// Assert Model on View is rendered with action verify
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="action"></param>
        /// <param name="viewName"></param>
        public void ShouldBeRenderModel<TModel>(Action<TModel> action, string viewName = "Action")
        {
            Action<ViewContext> verify = s => action((TModel)s.ViewData.Model);
            this.view.Verify(r => r.Render(Pleasure.MockIt.Is(verify), Pleasure.MockIt.IsAny<TextWriter>()));
            this.viewEngine.Verify(r => r.FindPartialView(Pleasure.MockIt.IsAny<ControllerContext>(), viewName, Pleasure.MockIt.IsAny<bool>()));
        }

        /// <summary>
        /// Mock Dispatcher.Push call
        /// </summary>
        /// <typeparam name="TCommand"></typeparam>
        /// <param name="command"></param>
        /// <param name="executeSetting"></param>
        /// <param name="callCount"></param>
        public void ShouldBePush<TCommand>(TCommand command, MessageExecuteSetting executeSetting = null, int callCount = 1) where TCommand : CommandBase
        {
            this.dispatcher.ShouldBePush(command, executeSetting, callCount);
        }

        /// <summary>
        /// Assert Dispatcher.Push wasn't called
        /// </summary>
        /// <typeparam name="TCommand"></typeparam>
        /// <param name="command"></param>
        public void ShouldNotBePush<TCommand>(TCommand command) where TCommand : CommandBase
        {
            ShouldBePush(command, callCount: 0);
        }

        /// <summary>
        /// Assert Dispatcher.Push call with action verifier
        /// </summary>
        /// <typeparam name="TCommand"></typeparam>
        /// <param name="action"></param>
        /// <param name="callCount"></param>
        public void ShouldBePush<TCommand>(Action<TCommand> action, int callCount = 1) where TCommand : CommandBase
        {
            this.dispatcher.ShouldBePush(action, callCount);
        }

        /// <summary>
        /// Generate invalid ModelState with ModelError
        /// </summary>
        /// <returns></returns>
        public MockController<TController> BrokenModelState()
        {
            var modelStateDictionary = new ModelStateDictionary();
            modelStateDictionary.AddModelError(Pleasure.Generator.String(), Pleasure.Generator.String());
            this.originalController.ViewData.SetValue("_modelState", modelStateDictionary);
            return this;
        }

        #endregion

        // Mock MVC ViewEngines
        void AddViewEngine()
        {
            this.view = Pleasure.Mock<IView>();
            this.viewEngine = Pleasure.Mock<IViewEngine>();
            this.viewEngine.Setup(r => r.FindPartialView(Pleasure.MockIt.IsAny<ControllerContext>(), Pleasure.MockIt.IsAny<string>(), Pleasure.MockIt.IsAny<bool>())).Returns(new ViewEngineResult(this.view.Object, this.viewEngine.Object));
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(this.viewEngine.Object);
        }
    }
}