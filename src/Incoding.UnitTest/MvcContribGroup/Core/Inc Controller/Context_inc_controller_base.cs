namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Incoding.CQRS;
    using Incoding.Extensions;
    using Incoding.MSpecContrib;
    using Incoding.MvcContrib;
    using Moq;

    #endregion

    public class Context_inc_controller_base
    {
        #region Constructors

        protected Context_inc_controller_base()
        {
            dispatcher = Pleasure.Mock<IDispatcher>();
            httpContext = Pleasure.Mock<HttpContextBase>();
            controller = new FakeController(dispatcher.Object, httpContext.Object);
            modelStateDictionary = new ModelStateDictionary();
            controller.ViewData.SetValue("_modelState", modelStateDictionary);
        }

        #endregion

        #region Nested classes

        protected class FakeCommand : CommandBase
        {
            public override void Execute() { }
        }

        protected class FakeController : IncControllerBase
        {
            #region Constructors

            public FakeController(IDispatcher dispatcher, HttpContextBase httpContextBase)
                    : base(dispatcher)
            {
                ControllerContext = new ControllerContext(httpContextBase, new RouteData(), this);
            }

            #endregion

            #region Api Methods

            public ActionResult Push(FakeCommand input)
            {
                return TryPush(composite => composite.Quote(input));
            }

            public ActionResult PushSuccessResult(FakeCommand input, Func<ActionResult> successResult)
            {
                return TryPush(input, setting => { setting.SuccessResult = successResult; });
            }

            public ActionResult PushErrorResult(FakeCommand input, Func<IncWebException, ActionResult> errorResult)
            {
                return TryPush(input, setting => { setting.ErrorResult = errorResult; });
            }

            #endregion
        }

        #endregion

        #region Estabilish value

        protected static FakeController controller;

        protected static Mock<IDispatcher> dispatcher;

        protected static ActionResult result;

        protected static Mock<HttpContextBase> httpContext;

        protected static ModelStateDictionary modelStateDictionary;

        #endregion
    }
}