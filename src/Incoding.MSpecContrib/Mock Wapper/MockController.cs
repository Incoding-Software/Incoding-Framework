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

    public class MockController<TController> where TController : IncControllerBase
    {
        #region Fields

        readonly Mock<IDispatcher> dispatcher;

        readonly TController originalController;

        readonly Mock<HttpContextBase> httpContext;

        Mock<IView> view;

        Mock<IViewEngine> viewEngine;

        #endregion

        #region Constructors

        MockController(TController controller, Mock<IDispatcher> dispatcher)
        {
            this.dispatcher = dispatcher;
            this.originalController = controller;

            this.httpContext = Pleasure.Mock<HttpContextBase>();

            var routeData = new RouteData();
            routeData.Values.Add("Action", "Action");
            routeData.Values.Add("Controller", "Controller");

            this.originalController.ControllerContext = new ControllerContext(this.httpContext.Object, routeData, Pleasure.Mock<ControllerBase>().Object);

            var routes = new RouteCollection();
            routes.MapRoute(name: "Default", 
                            url: "{controller}/{action}/{id}", 
                            defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional });

            this.originalController.Url = new UrlHelper(new RequestContext(this.httpContext.Object, routeData), routes);

            AddViewEngine();
        }

        #endregion

        #region Factory constructors

        public static MockController<TController> When(params object[] ctorArgs)
        {
            var dispatcher = Pleasure.Mock<IDispatcher>();
            IoCFactory.Instance.StubTryResolve(dispatcher.Object);

            var controller = (TController)Activator.CreateInstance(typeof(TController), ctorArgs.ToArray());
            var res = new MockController<TController>(controller, dispatcher);
            res.httpContext.SetupGet(r => r.Request.Headers).Returns(new NameValueCollection { { "X-Requested-With", "XMLHttpRequest" } });

            return res;
        }

        #endregion

        #region Properties

        public TController Original { get { return this.originalController; } }

        #endregion

        #region Api Methods

        public MockController<TController> DisableAjax()
        {
            return SetupHttpContext(mock => mock.SetupGet(r => r.Request.Headers).Returns(new NameValueCollection()));
        }

        public MockController<TController> StubRequestUrl(Uri requestUri)
        {
            return SetupHttpContext(mock =>
                                        {
                                            mock.SetupGet(r => r.Request.Url).Returns(requestUri);
                                            mock.SetupGet(r => r.Request.UrlReferrer).Returns(requestUri);
                                        });
        }

        public MockController<TController> SetupHttpContext(Action<Mock<HttpContextBase>> action)
        {
            action(this.httpContext);
            return this;
        }

        public MockController<TController> StubUrlAction(string expectedRoute)
        {
            return SetupHttpContext(mock =>
                                        {
                                            mock.Setup(r => r.Response.ApplyAppPathModifier(Pleasure.MockIt.IsStrong(expectedRoute))).Returns(expectedRoute);
                                            mock.Setup(r => r.Request.ApplicationPath).Returns("/");
                                        });
        }

        public MockController<TController> StubUrlAction(Action<string> verifyRoutes, string expectedRoute)
        {
            return SetupHttpContext(mock =>
                                        {
                                            mock.Setup(r => r.Response.ApplyAppPathModifier(Pleasure.MockIt.Is(verifyRoutes))).Returns(expectedRoute);
                                            mock.Setup(r => r.Request.ApplicationPath).Returns("/");
                                        });
        }

        public MockController<TController> StubQuery<TQuery, TResult>(TQuery query, TResult result, MessageExecuteSetting executeSetting = null) where TQuery : QueryBase<TResult> where TResult : class
        {
            this.dispatcher.StubQuery(query, result, executeSetting);
            return this;
        }

        public MockController<TController> StubPrincipal(IPrincipal principal)
        {
            return SetupHttpContext(mock => mock.Setup(r => r.User).Returns(principal));
        }

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

        public MockController<TController> StubPushAsThrow<TCommand>(TCommand command, Exception exception, MessageExecuteSetting executeSetting = null) where TCommand : CommandBase
        {
            this.dispatcher.StubPushAsThrow(command, exception, executeSetting);
            return this;
        }

        public MockController<TController> StubQueryAsThrow<TQuery, TResult>(TQuery query, Exception exception) where TQuery : QueryBase<TResult> where TResult : class
        {
            this.dispatcher.StubQueryAsThrow<TQuery, TResult>(query, exception);
            return this;
        }

        public void ShouldBeRenderView(string viewName = "Action")
        {
            ShouldBeRenderModel(null, viewName);
        }

        public void ShouldBeRenderModel(object model, string viewName = "Action")
        {
            ShouldBeRenderModel<object>(o => o.ShouldEqualWeak(model), viewName);
        }

        public void ShouldBeRenderModel<TModel>(Action<TModel> action, string viewName = "Action")
        {
            Action<ViewContext> verify = s => action((TModel)s.ViewData.Model);
            this.view.Verify(r => r.Render(Pleasure.MockIt.Is(verify), Pleasure.MockIt.IsAny<TextWriter>()));
            this.viewEngine.Verify(r => r.FindPartialView(Pleasure.MockIt.IsAny<ControllerContext>(), viewName, Pleasure.MockIt.IsAny<bool>()));
        }

        public void ShouldBePush<TCommand>(TCommand command, MessageExecuteSetting executeSetting = null, int callCount = 1) where TCommand : CommandBase
        {
            this.dispatcher.ShouldBePush(command, executeSetting, callCount);
        }

        public void ShouldNotBePush<TCommand>(TCommand command) where TCommand : CommandBase
        {
            ShouldBePush(command, callCount: 0);
        }

        public void ShouldBePush<TCommand>(Action<TCommand> action, int callCount = 1) where TCommand : CommandBase
        {
            this.dispatcher.ShouldBePush(action, callCount);
        }

        public MockController<TController> BrokenModelState()
        {
            var modelStateDictionary = new ModelStateDictionary();
            modelStateDictionary.AddModelError(Pleasure.Generator.String(), Pleasure.Generator.String());
            this.originalController.ViewData.SetValue("_modelState", modelStateDictionary);
            return this;
        }

        #endregion

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