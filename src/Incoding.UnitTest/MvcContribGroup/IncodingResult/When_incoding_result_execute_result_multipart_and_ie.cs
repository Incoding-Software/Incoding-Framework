namespace Incoding.UnitTest.MvcContribGroup
{
    using System.Net;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Incoding.Extensions;
    using Incoding.MSpecContrib;
    using Incoding.MvcContrib;
    using Machine.Specifications;
    using Machine.Specifications.Annotations;
    using Moq;
    using It = Machine.Specifications.It;

    [Subject(typeof(IncodingResult))]
    public class When_incoding_result_execute_result_multipart_and_ie
    {
        Establish establish = () =>
                              {
                                  fake = Pleasure.Generator.Invent<FakeProp>();
                                  response = Pleasure.Mock<HttpResponseBase>(mock => mock.SetupSet(r => r.ContentType = "text/html"));
                                  var contextBase = Pleasure.MockStrictAsObject<HttpContextBase>(mock =>
                                                                                                 {
                                                                                                     mock.Setup(r => r.Response).Returns(response.Object);
                                                                                                     mock.SetupGet(r => r.Request.Browser.Browser).Returns("IE");
                                                                                                     mock.SetupGet(r => r.Request.ContentType).Returns("multipart/form-data");
                                                                                                 });
                                  controllerContext = new ControllerContext(contextBase, new RouteData(), Pleasure.MockAsObject<ControllerBase>());
                              };

        Because of = () => IncodingResult.Success(fake)
                                         .ExecuteResult(controllerContext);

        It should_be_execute_result = () => response.Verify(r => r.Write(new IncodingResult.JsonData(true, fake, string.Empty,HttpStatusCode.OK).ToJsonString()));

        #region Fake classes

        public class FakeProp
        {
            #region Properties

            [UsedImplicitly]
            public string Prop { get; set; }

            #endregion
        }

        #endregion

        #region Establish value

        static ControllerContext controllerContext;

        static Mock<HttpResponseBase> response;

        static FakeProp fake;

        #endregion
    }
}