namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System;
    using System.Collections.Specialized;
    using System.Reflection;
    using System.Threading;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Incoding.Block.IoC;
    using Incoding.CQRS;
    using Incoding.MSpecContrib;
    using Incoding.MvcContrib.MVD;
    using Moq;

    #endregion

    public class Context_dispatcher_controller
    {
        static Context_dispatcher_controller()
        {
            dispatcher = Pleasure.Mock<IDispatcher>();

            IoCFactory.Instance.StubTryResolve(dispatcher.Object);
            controller = new FakeDispatcher(Assembly.GetCallingAssembly());

            requestBase = Pleasure.Mock<HttpRequestBase>(mock => { mock.SetupGet(r => r.Headers).Returns(new NameValueCollection { { "X-Requested-With", "XMLHttpRequest" } }); });

            responseBase = Pleasure.MockStrict<HttpResponseBase>();
            controller.ControllerContext = new ControllerContext(Pleasure.MockStrictAsObject<HttpContextBase>(mock =>
                                                                                                              {
                                                                                                                  mock.SetupGet(r => r.Request).Returns(requestBase.Object);
                                                                                                                  mock.SetupGet(r => r.Response).Returns(responseBase.Object);
                                                                                                              }), new RouteData(), controller);
            controller.ValueProvider = Pleasure.MockStrictAsObject<IValueProvider>(mock => mock.Setup(r => r.GetValue(Pleasure.MockIt.IsAny<string>())).Returns(new ValueProviderResult(string.Empty, string.Empty, Thread.CurrentThread.CurrentCulture)));
        }

        public class FakeCommand : CommandBase
        {
            public string Name { get; set; }

            protected override void Execute()
            {
                throw new NotImplementedException();
            }
        }

        public class FakeFileByNameQuery : QueryBase<byte[]>
        {
            ////ncrunch: no coverage start
            protected override byte[] ExecuteResult()
            {
                throw new NotImplementedException();
            }

            ////ncrunch: no coverage end        
        }

        public class ShareGenericQuery<T> : QueryBase<T>
        {
            ////ncrunch: no coverage start
            protected override T ExecuteResult()
            {
                throw new NotImplementedException();
            }

            ////ncrunch: no coverage end        
        }

        public class ShareQuery : QueryBase<string>
        {
            protected override string ExecuteResult()
            {
                throw new NotImplementedException();
            }
        }

        #region Fake classes

        public class FakeDispatcher : DispatcherControllerBase
        {
            public FakeDispatcher(Assembly type, Func<Type, bool> filterTypes = null)
                    : base(type, filterTypes) { }

            public FakeDispatcher(Assembly[] assemblies, Func<Type, bool> filterTypes = null)
                    : base(assemblies, filterTypes) { }
        }

        #endregion

        public class FakeModel { }

        #region Establish value

        protected static FakeDispatcher controller;

        protected static Mock<IDispatcher> dispatcher;

        protected static ActionResult result;

        protected static Mock<HttpRequestBase> requestBase;

        protected static Mock<HttpResponseBase> responseBase;

        #endregion
    }
}