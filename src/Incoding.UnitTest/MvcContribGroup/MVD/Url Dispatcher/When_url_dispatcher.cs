namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Incoding.CQRS;
    using Incoding.Data;
    using Incoding.MSpecContrib;
    using Incoding.MvcContrib;
    using Incoding.MvcContrib.MVD;
    using Machine.Specifications;
    using Moq;
    using It = Machine.Specifications.It;

    #endregion

    class DuplicateCommand { }

    [Subject(typeof(UrlDispatcher))]
    public class When_url_dispatcher
    {
        Establish establish = () =>
                              {
                                  var routeData = new RouteData();
                                  routeData.Values.Add("Action", "Action");
                                  routeData.Values.Add("Controller", "Controller");

                                  var routes = new RouteCollection();
                                  routes.MapRoute(name: "Default",
                                                  url: "{controller}/{action}/{id}",
                                                  defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional });

                                  httpContext = Pleasure.Mock<HttpContextBase>(mock => mock.Setup(r => r.Request.ApplicationPath).Returns("/"));
                                  var urlHelper = new UrlHelper(new RequestContext(httpContext.Object, routeData), routes);
                                  urlDispatcher = new UrlDispatcher(urlHelper);
                              };

        It should_be_model_generic_to_view = () =>
                                             {
                                                 const string actionUrl = "/Dispatcher/Render?incType=FakeModelT%601%7CString&incIsModel=True&incView=~%2FFakeSerializeObject.cs";
                                                 httpContext.Setup(r => r.Response.ApplyAppPathModifier(Pleasure.MockIt.IsStrong(actionUrl))).Returns(actionUrl);
                                                 urlDispatcher.Model<FakeModelT<string>>()
                                                              .AsView("~/FakeSerializeObject.cs")
                                                              .ShouldEqual(actionUrl);
                                             };

        It should_be_model_null_to_view = () =>
                                          {
                                              const string actionUrl = "/Dispatcher/Render?incType=When_Url_Dispatcher_FakeModel&incIsModel=True&incView=~%2FFakeSerializeObject.cs";
                                              httpContext.Setup(r => r.Response.ApplyAppPathModifier(Pleasure.MockIt.IsStrong(actionUrl))).Returns(actionUrl);
                                              urlDispatcher.Model<When_Url_Dispatcher_FakeModel>()
                                                           .AsView("~/FakeSerializeObject.cs")
                                                           .ShouldEqual(actionUrl);
                                          };

        It should_be_model_to_view = () =>
                                     {
                                         const string actionUrl = "/Dispatcher/Render?incType=When_Url_Dispatcher_FakeModel&incIsModel=True&incView=~%2FFakeSerializeObject.cs";
                                         httpContext.Setup(r => r.Response.ApplyAppPathModifier(Pleasure.MockIt.IsStrong(actionUrl))).Returns(actionUrl);
                                         urlDispatcher.Model(new When_Url_Dispatcher_FakeModel { DecodeValue = "{{1}}", EncodeValue = HttpUtility.UrlEncode("{{1}}") })
                                                      .AsView("~/FakeSerializeObject.cs")
                                                      .ShouldEqual("/Dispatcher/Render?incType=When_Url_Dispatcher_FakeModel&incIsModel=True&incView=~%2FFakeSerializeObject.cs&EncodeValue=%7b%7b1%7d%7d&DecodeValue={{1}}");
                                     };

        It should_be_model_typical_to_view = () =>
                                             {
                                                 const string actionUrl = "/Dispatcher/Render?incType=String&incIsModel=True&incView=~%2FFakeSerializeObject.cs";
                                                 httpContext.Setup(r => r.Response.ApplyAppPathModifier(Pleasure.MockIt.IsStrong(actionUrl))).Returns(actionUrl);
                                                 urlDispatcher.Model("value")
                                                              .AsView("~/FakeSerializeObject.cs")
                                                              .ShouldEqual("/Dispatcher/Render?incType=String&incIsModel=True&incView=~%2FFakeSerializeObject.cs&incValue=value");
                                             };

        It should_be_model_with_selector_to_view = () =>
                                                   {
                                                       const string actionUrl = "/Dispatcher/Render?incType=When_Url_Dispatcher_FakeModel&incIsModel=True&incView=~%2FFakeSerializeObject.cs";
                                                       httpContext.Setup(r => r.Response.ApplyAppPathModifier(Pleasure.MockIt.IsStrong(actionUrl))).Returns(actionUrl);
                                                       string asView = urlDispatcher.Model<When_Url_Dispatcher_FakeModel>(new
                                                                                                                          {
                                                                                                                                  GapId = Selector.Incoding.AjaxPost("url?test=value1&test2=value2")
                                                                                                                          })
                                                                                    .AsView("~/FakeSerializeObject.cs");
                                                       asView.ShouldEqual("/Dispatcher/Render?incType=When_Url_Dispatcher_FakeModel&incIsModel=True&incView=~%2FFakeSerializeObject.cs&GapId=||ajax*{\"url\":\"url?test=value1&test2=value2\",\"type\":\"POST\",\"async\":false}||");                                                       
                                                   };

        It should_be_push = () =>
                            {
                                const string actionUrl = "/Dispatcher/Push?incTypes=When_Url_Dispatcher_FakeCommand";
                                httpContext.Setup(r => r.Response.ApplyAppPathModifier(Pleasure.MockIt.IsStrong(actionUrl))).Returns(actionUrl);
                                urlDispatcher.Push(new When_Url_Dispatcher_FakeCommand
                                                   {
                                                           DecodeValue = "{{1}}",
                                                           EncodeValue = HttpUtility.UrlEncode("{{1}}"),
                                                   })
                                             .ToString()
                                             .ShouldEqual("/Dispatcher/Push?incTypes=When_Url_Dispatcher_FakeCommand&EncodeValue=%7b%7b1%7d%7d&DecodeValue={{1}}");
                            };

        It should_be_push_anonymous = () =>
                                      {
                                          const string actionUrl = "/Dispatcher/Push?incTypes=When_Url_Dispatcher_FakeCommand";
                                          httpContext.Setup(r => r.Response.ApplyAppPathModifier(Pleasure.MockIt.IsStrong(actionUrl))).Returns(actionUrl);
                                          var result = urlDispatcher.Push<When_Url_Dispatcher_FakeCommand>(new
                                                                                                           {
                                                                                                                   DecodeValue = "{{1}}",
                                                                                                                   EncodeValue = HttpUtility.UrlEncode("{{1}}"),
                                                                                                           })
                                                                    .ToString();
                                          result.ShouldEqual("/Dispatcher/Push?incTypes=When_Url_Dispatcher_FakeCommand&DecodeValue={{1}}&EncodeValue=%7b%7b1%7d%7d");
                                      };

        It should_be_push_as_string = () =>
                                      {
                                          const string actionUrl = "/Dispatcher/Push?incTypes=When_Url_Dispatcher_FakeCommand";
                                          httpContext.Setup(r => r.Response.ApplyAppPathModifier(Pleasure.MockIt.IsStrong(actionUrl))).Returns(actionUrl);
                                          string pushAsString = urlDispatcher.Push(new When_Url_Dispatcher_FakeCommand
                                                                                   {
                                                                                           DecodeValue = "{{1}}",
                                                                                           EncodeValue = HttpUtility.UrlEncode("{{1}}"),
                                                                                   });
                                          pushAsString.ShouldEqual("/Dispatcher/Push?incTypes=When_Url_Dispatcher_FakeCommand&EncodeValue=%7b%7b1%7d%7d&DecodeValue={{1}}");
                                      };

        It should_be_push_command_with_property_only_get = () =>
                                                           {
                                                               const string actionUrl = "/Dispatcher/Push?incTypes=When_Url_Dispatcher_FakeCommand_With_property_get";
                                                               httpContext.Setup(r => r.Response.ApplyAppPathModifier(Pleasure.MockIt.IsStrong(actionUrl))).Returns(actionUrl);
                                                               string pushAsString = urlDispatcher.Push(new When_Url_Dispatcher_FakeCommand_With_property_get()
                                                                                                        {
                                                                                                                DecodeValue = "1"
                                                                                                        });
                                                               pushAsString.ShouldEqual("/Dispatcher/Push?incTypes=When_Url_Dispatcher_FakeCommand_With_property_get&DecodeValue=1");
                                                           };

        It should_be_push_composite = () =>
                                      {
                                          const string actionUrl = "/Dispatcher/Push?incTypes=When_Url_Dispatcher_FakeCommand%26When_Url_Dispatcher_FakeCommand2";
                                          httpContext.Setup(r => r.Response.ApplyAppPathModifier(Pleasure.MockIt.IsStrong(actionUrl))).Returns(actionUrl);
                                          urlDispatcher.Push(new When_Url_Dispatcher_FakeCommand { DecodeValue = "{{1}}" })
                                                       .Push(new When_Url_Dispatcher_FakeCommand2 { Command2Value = Pleasure.Generator.TheSameString() })
                                                       .ToString()
                                                       .ShouldEqual("/Dispatcher/Push?incTypes=When_Url_Dispatcher_FakeCommand%26When_Url_Dispatcher_FakeCommand2&DecodeValue={{1}}&Command2Value=TheSameString");
                                      };

        It should_be_push_composite_as_array = () =>
                                               {
                                                   const string actionUrl = "/Dispatcher/Push?incTypes=When_Url_Dispatcher_FakeCommand&incIsCompositeAsArray=True";
                                                   httpContext.Setup(r => r.Response.ApplyAppPathModifier(Pleasure.MockIt.IsStrong(actionUrl))).Returns(actionUrl);
                                                   urlDispatcher.Push(new When_Url_Dispatcher_FakeCommand { DecodeValue = "value1" })
                                                                .Push(new When_Url_Dispatcher_FakeCommand { DecodeValue = "value2" })
                                                                .ToString()
                                                                .ShouldEqual("/Dispatcher/Push?incTypes=When_Url_Dispatcher_FakeCommand&incIsCompositeAsArray=True&%5b0%5d.DecodeValue=value1&%5b1%5d.DecodeValue=value2");
                                               };

        It should_be_push_generic = () =>
                                    {
                                        const string actionUrl = "/Dispatcher/Push?incTypes=FakeGenericCommand%601%7CIncEntityBase";
                                        httpContext.Setup(r => r.Response.ApplyAppPathModifier(Pleasure.MockIt.IsStrong(actionUrl))).Returns(actionUrl);
                                        urlDispatcher.Push<FakeGenericCommand<IncEntityBase>>()
                                                     .ToString()
                                                     .ShouldEqual(actionUrl);
                                    };

        It should_be_push_multiple_generic = () =>
                                             {
                                                 const string actionUrl = "/Dispatcher/Push?incTypes=FakeMultipleGenericQuery%602%7CString%2FInt32";
                                                 httpContext.Setup(r => r.Response.ApplyAppPathModifier(Pleasure.MockIt.IsStrong(actionUrl))).Returns(actionUrl);
                                                 urlDispatcher.Push<FakeMultipleGenericQuery<string, int>>()
                                                              .ToString()
                                                              .ShouldEqual(actionUrl);
                                             };

        It should_be_push_only_validate = () =>
                                          {
                                              const string actionUrl = "/Dispatcher/Push?incTypes=When_Url_Dispatcher_FakeCommand&incOnlyValidate=True";
                                              httpContext.Setup(r => r.Response.ApplyAppPathModifier(Pleasure.MockIt.IsStrong(actionUrl))).Returns(actionUrl);
                                              urlDispatcher.Push(new When_Url_Dispatcher_FakeCommand
                                                                 {
                                                                         DecodeValue = "{{1}}",
                                                                         EncodeValue = HttpUtility.UrlEncode("{{1}}"),
                                                                 })
                                                           .OnlyValidate()
                                                           .ToString()
                                                           .ShouldEqual("/Dispatcher/Push?incTypes=When_Url_Dispatcher_FakeCommand&incOnlyValidate=True&EncodeValue=%7b%7b1%7d%7d&DecodeValue={{1}}");
                                          };

        It should_be_push_with_duplicate = () =>
                                           {
                                               const string actionUrl = "/Dispatcher/Push?incTypes=Incoding.UnitTest.MvcContribGroup.When_url_dispatcher%2BDuplicateCommand";
                                               httpContext.Setup(r => r.Response.ApplyAppPathModifier(Pleasure.MockIt.IsStrong(actionUrl))).Returns(actionUrl);
                                               urlDispatcher.Push(new DuplicateCommand())
                                                            .ToString()
                                                            .ShouldEqual(actionUrl);
                                           };

        It should_be_query_enable_validate_as_json = () =>
                                                     {
                                                         const string actionUrl = "/Dispatcher/Query?incType=When_Url_dispatcher_FakeQuery&incValidate=True";
                                                         httpContext.Setup(r => r.Response.ApplyAppPathModifier(Pleasure.MockIt.IsStrong(actionUrl))).Returns(actionUrl);
                                                         urlDispatcher.Query(new When_Url_dispatcher_FakeQuery())
                                                                      .EnableValidate()
                                                                      .AsJson()
                                                                      .ShouldEqual(actionUrl);
                                                     };

        It should_be_query_multiple_generic = () =>
                                              {
                                                  const string actionUrl = "/Dispatcher/Query?incType=FakeMultipleGenericQuery%602%7CIncEntityBase%2FString";
                                                  httpContext.Setup(r => r.Response.ApplyAppPathModifier(actionUrl)).Returns(actionUrl);
                                                  urlDispatcher.Query(new FakeMultipleGenericQuery<IncEntityBase, string>())
                                                               .AsJson()
                                                               .ShouldEqual(actionUrl);
                                              };

        It should_be_query_single_generic = () =>
                                            {
                                                const string actionUrl = "/Dispatcher/Query?incType=FakeGenericQuery%601%7CIncEntityBase";
                                                httpContext.Setup(r => r.Response.ApplyAppPathModifier(Pleasure.MockIt.IsStrong(actionUrl))).Returns(actionUrl);
                                                urlDispatcher.Query(new FakeGenericQuery<IncEntityBase>())
                                                             .AsJson()
                                                             .ShouldEqual(actionUrl);
                                            };

        It should_be_query_to_file = () =>
                                     {
                                         const string actionUrl = "/Dispatcher/QueryToFile?incType=FakeBytesQuery";
                                         httpContext.Setup(r => r.Response.ApplyAppPathModifier(Pleasure.MockIt.IsStrong(actionUrl))).Returns(actionUrl);
                                         urlDispatcher.Query(new FakeBytesQuery
                                                             {
                                                                     DecodeValue = "{{1}}",
                                                                     EncodeValue = HttpUtility.UrlEncode("{{1}}"),
                                                             })
                                                      .AsFile(incContentType: "{{img}}", incFileDownloadName: "{{file}}")
                                                      .ShouldEqual("/Dispatcher/QueryToFile?incType=FakeBytesQuery&incContentType={{img}}&incFileDownloadName={{file}}&EncodeValue=%7b%7b1%7d%7d&DecodeValue={{1}}");
                                     };

        It should_be_query_to_json = () =>
                                     {
                                         const string actionUrl = "/Dispatcher/Query?incType=When_Url_dispatcher_FakeQuery";
                                         httpContext.Setup(r => r.Response.ApplyAppPathModifier(Pleasure.MockIt.IsStrong(actionUrl))).Returns(actionUrl);
                                         urlDispatcher.Query(new When_Url_dispatcher_FakeQuery
                                                             {
                                                                     DecodeValue = "{{1}}",
                                                                     EncodeValue = HttpUtility.UrlEncode("{{1}}"),
                                                             })
                                                      .AsJson()
                                                      .ShouldEqual("/Dispatcher/Query?incType=When_Url_dispatcher_FakeQuery&EncodeValue=%7b%7b1%7d%7d&DecodeValue={{1}}");
                                     };

        It should_be_query_to_string = () =>
                                       {
                                           var query = urlDispatcher.Query(new When_Url_dispatcher_FakeQuery
                                                                           {
                                                                                   DecodeValue = "{{1}}", EncodeValue = HttpUtility.UrlEncode("{{1}}"),
                                                                           });
                                           query.ToString().ShouldEqual(query.AsJson());
                                       };

        It should_be_query_to_view = () =>
                                     {
                                         const string actionUrl = "/Dispatcher/Render?incType=When_Url_dispatcher_FakeQuery&incView=~%2FFakeSerializeObject.cs";
                                         httpContext.Setup(r => r.Response.ApplyAppPathModifier(Pleasure.MockIt.IsStrong(actionUrl))).Returns(actionUrl);
                                         urlDispatcher.Query(new When_Url_dispatcher_FakeQuery())
                                                      .AsView("~/FakeSerializeObject.cs")
                                                      .ShouldEqual(actionUrl);
                                     };

        It should_be_query_validate_only_as_json = () =>
                                                   {
                                                       const string actionUrl = "/Dispatcher/Validate?incType=When_Url_dispatcher_FakeQuery";
                                                       httpContext.Setup(r => r.Response.ApplyAppPathModifier(Pleasure.MockIt.IsStrong(actionUrl))).Returns(actionUrl);
                                                       urlDispatcher.Query(new When_Url_dispatcher_FakeQuery())
                                                                    .Validate()
                                                                    .ShouldEqual(actionUrl);
                                                   };

        It should_be_render = () =>
                              {
                                  const string actionUrl = "/Dispatcher/Render?incView=~%2FFakeSerializeObject.cs";
                                  httpContext.Setup(r => r.Response.ApplyAppPathModifier(Pleasure.MockIt.IsStrong(actionUrl))).Returns(actionUrl);
                                  urlDispatcher.AsView("~/FakeSerializeObject.cs")
                                               .ShouldEqual(actionUrl);
                              };

        #region Fake classes

        class DuplicateCommand { }

        class When_Url_Dispatcher_FakeCommand
        {
            #region Properties

            public string EncodeValue { get; set; }

            public string DecodeValue { get; set; }

            #endregion
        }

        class When_Url_Dispatcher_FakeCommand_With_property_get
        {
            #region Properties

            public string EncodeValue { get { throw new ArgumentException(); } }

            public string DecodeValue { get; set; }

            #endregion
        }

        class When_Url_Dispatcher_FakeCommand2
        {
            #region Properties

            public string Command2Value { get; set; }

            #endregion
        }

        class FakeGenericCommand<TEntity> { }

        class When_Url_dispatcher_FakeQuery : QueryBase<string>
        {
            ////ncrunch: no coverage start
            protected override string ExecuteResult()
            {
                throw new NotImplementedException();
            }

            ////ncrunch: no coverage end        

            #region Properties

            public string EncodeValue { get; set; }

            public string DecodeValue { get; set; }

            #endregion
        }

        class FakeGenericQuery<T> : QueryBase<string>
        {
            ////ncrunch: no coverage start
            protected override string ExecuteResult()
            {
                throw new NotImplementedException();
            }

            ////ncrunch: no coverage end        
        }

        class FakeMultipleGenericQuery<T, T2> : QueryBase<string>
        {
            ////ncrunch: no coverage start
            protected override string ExecuteResult()
            {
                throw new NotImplementedException();
            }

            ////ncrunch: no coverage end        
        }

        class FakeBytesQuery : QueryBase<byte[]>
        {
            ////ncrunch: no coverage start
            protected override byte[] ExecuteResult()
            {
                throw new NotImplementedException();
            }

            ////ncrunch: no coverage end        

            #region Properties

            public string EncodeValue { get; set; }

            public string DecodeValue { get; set; }

            #endregion
        }

        public class When_Url_Dispatcher_FakeModel
        {
            #region Properties

            public string EncodeValue { get; set; }

            public string DecodeValue { get; set; }

            #endregion
        }

        public class FakeModelT<T>
        {
            #region Properties

            public string EncodeValue { get; set; }

            public string DecodeValue { get; set; }

            #endregion
        }

        #endregion

        #region Establish value

        static UrlDispatcher urlDispatcher;

        static Mock<HttpContextBase> httpContext;

        #endregion
    }
}