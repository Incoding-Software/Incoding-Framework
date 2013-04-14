namespace Incoding.MSpecContrib
{
    using System;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Moq;

    #region << Using >>

    

    #endregion

    public class MockUrl
    {
        #region Fields

        readonly Mock<HttpContextBase> httpContext;

        #endregion

        #region Constructors

        public MockUrl()
        {
            var routeData = new RouteData();
            routeData.Values.Add("Action", "Action");
            routeData.Values.Add("Controller", "Controller");
            this.httpContext = Pleasure.Mock<HttpContextBase>(mock =>
                                                                  {
                                                                      var httpRequestBase = Pleasure.MockAsObject<HttpRequestBase>(mock1 => mock1.SetupGet(r => r.ApplicationPath).Returns("/"));
                                                                      mock.SetupGet(r => r.Request).Returns(httpRequestBase);
                                                                  });
            var requestContext = new RequestContext(this.httpContext.Object, routeData);

            var routes = new RouteCollection();
            routes.MapRoute(
                            name: "Default", 
                            url: "{controller}/{action}/{id}", 
                            defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional });
            this.Original = new UrlHelper(requestContext, routes);
        }

        #endregion

        #region Properties

        public UrlHelper Original { get; private set; }

        #endregion

        #region Api Methods

        public MockUrl StubAction(string url)
        {
            this.httpContext
                .SetupGet(r => r.Response)
                .Returns(Pleasure.MockAsObject<HttpResponseBase>(mock1 => mock1.Setup(r => r.ApplyAppPathModifier(Pleasure.MockIt.IsStrong(url))).Returns(url)));
            return this;
        }

        public MockUrl StubRequestUrl(Uri uri)
        {
            this.httpContext.SetupGet(r => r.Request.Url).Returns(uri);
            this.httpContext.SetupGet(r => r.Request.UrlReferrer).Returns(uri);
            return this;
        }

        #endregion
    }
}