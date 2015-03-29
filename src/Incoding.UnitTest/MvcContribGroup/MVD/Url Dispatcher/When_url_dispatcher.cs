namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Incoding.CQRS;
    using Incoding.Data;
    using Incoding.MSpecContrib;
    using Incoding.MvcContrib.MVD;
    using Machine.Specifications;
    using Moq;
    using It = Machine.Specifications.It;

    #endregion

    [Subject(typeof(UrlDispatcher))]
    public class When_url_dispatcher
    {
        #region Fake classes

        class DuplicateCommand { }

        class FakeCommand
        {
            #region Properties

            public string EncodeValue { get; set; }

            public string DecodeValue { get; set; }

            #endregion
        }

        class FakeCommand2
        {
            #region Properties

            public string Command2Value { get; set; }

            #endregion
        }

        class FakeGenericCommand<TEntity> { }

        class FakeQuery : QueryBase<string>
        {
            #region Properties

            public string EncodeValue { get; set; }

            public string DecodeValue { get; set; }

            #endregion

            ////ncrunch: no coverage start
            protected override string ExecuteResult()
            {
                throw new NotImplementedException();
            }

            ////ncrunch: no coverage end        
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
            #region Properties

            public string EncodeValue { get; set; }

            public string DecodeValue { get; set; }

            #endregion

            ////ncrunch: no coverage start
            protected override byte[] ExecuteResult()
            {
                throw new NotImplementedException();
            }

            ////ncrunch: no coverage end        
        }

        public class FakeModel
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
                                      typeof(DispatcherControllerBase).GetField("duplicates", BindingFlags.Static | BindingFlags.NonPublic)
                                                                      .SetValue(null, new List<Type> { typeof(DuplicateCommand) });
                                  };

        It should_be_push_composite = () =>
                                          {
                                              const string actionUrl = "/Dispatcher/Composite?incTypes=FakeCommand%2CFakeCommand2";
                                              httpContext.Setup(r => r.Response.ApplyAppPathModifier(Pleasure.MockIt.IsStrong(actionUrl))).Returns(actionUrl);
                                              urlDispatcher.Push(new FakeCommand { DecodeValue = "{{1}}" })
                                                           .Push(new FakeCommand2 { Command2Value = Pleasure.Generator.TheSameString() })
                                                           .ToString()
                                                           .ShouldEqual("/Dispatcher/Composite?incTypes=FakeCommand%2CFakeCommand2&DecodeValue={{1}}&Command2Value=TheSameString");
                                          };

        It should_be_push_same_command_on_composite = () =>
                                                          {
                                                              const string actionUrl = "/Dispatcher/Composite?incTypes=FakeCommand";
                                                              httpContext.Setup(r => r.Response.ApplyAppPathModifier(Pleasure.MockIt.IsStrong(actionUrl))).Returns(actionUrl);
                                                              urlDispatcher.Push(new FakeCommand { DecodeValue = "value1" }).Push(new FakeCommand { DecodeValue = "value2" })
                                                                           .ToString()
                                                                           .ShouldEqual("/Dispatcher/Composite?incTypes=FakeCommand&%5b0%5d.DecodeValue=value1&%5b1%5d.DecodeValue=value2");
                                                          };

        It should_be_push = () =>
                                {
                                    const string actionUrl = "/Dispatcher/Push?incType=FakeCommand";
                                    httpContext.Setup(r => r.Response.ApplyAppPathModifier(Pleasure.MockIt.IsStrong(actionUrl))).Returns(actionUrl);
                                    urlDispatcher.Push(new FakeCommand
                                                           {
                                                                   DecodeValue = "{{1}}",
                                                                   EncodeValue = HttpUtility.UrlEncode("{{1}}"),
                                                           })
                                                 .ToString()
                                                 .ShouldEqual("/Dispatcher/Push?incType=FakeCommand&EncodeValue=%7b%7b1%7d%7d&DecodeValue={{1}}");
                                };

        It should_be_push_as_string = () =>
                                          {
                                              const string actionUrl = "/Dispatcher/Push?incType=FakeCommand";
                                              httpContext.Setup(r => r.Response.ApplyAppPathModifier(Pleasure.MockIt.IsStrong(actionUrl))).Returns(actionUrl);
                                              string pushAsString = urlDispatcher.Push(new FakeCommand
                                                                                           {
                                                                                                   DecodeValue = "{{1}}",
                                                                                                   EncodeValue = HttpUtility.UrlEncode("{{1}}"),
                                                                                           });
                                              pushAsString.ShouldEqual("/Dispatcher/Push?incType=FakeCommand&EncodeValue=%7b%7b1%7d%7d&DecodeValue={{1}}");
                                          };

        It should_be_push_with_duplicate = () =>
                                               {
                                                   const string actionUrl = "/Dispatcher/Push?incType=Incoding.UnitTest.MvcContribGroup.When_url_dispatcher%2BDuplicateCommand";
                                                   httpContext.Setup(r => r.Response.ApplyAppPathModifier(Pleasure.MockIt.IsStrong(actionUrl))).Returns(actionUrl);
                                                   urlDispatcher.Push(new DuplicateCommand())
                                                                .ToString()
                                                                .ShouldEqual(actionUrl);
                                               };

        It should_be_push_generic = () =>
                                        {
                                            const string actionUrl = "/Dispatcher/Push?incType=FakeGenericCommand%601&incGeneric=IncEntityBase";
                                            httpContext.Setup(r => r.Response.ApplyAppPathModifier(Pleasure.MockIt.IsStrong(actionUrl))).Returns(actionUrl);
                                            urlDispatcher.Push<FakeGenericCommand<IncEntityBase>>()
                                                         .ToString()
                                                         .ShouldEqual(actionUrl);
                                        };

        It should_be_render = () =>
                                  {
                                      const string actionUrl = "/Dispatcher/Render?incView=~%2FFakeSerializeObject.cs";
                                      httpContext.Setup(r => r.Response.ApplyAppPathModifier(Pleasure.MockIt.IsStrong(actionUrl))).Returns(actionUrl);
                                      urlDispatcher.AsView("~/FakeSerializeObject.cs")
                                                   .ShouldEqual(actionUrl);
                                  };

        It should_be_model_null_to_view = () =>
                                              {
                                                  const string actionUrl = "/Dispatcher/Render?incType=FakeModel&incIsModel=True&incView=~%2FFakeSerializeObject.cs";
                                                  httpContext.Setup(r => r.Response.ApplyAppPathModifier(Pleasure.MockIt.IsStrong(actionUrl))).Returns(actionUrl);
                                                  urlDispatcher.Model<FakeModel>()
                                                               .AsView("~/FakeSerializeObject.cs")
                                                               .ShouldEqual(actionUrl);
                                              };

        It should_be_model_to_view = () =>
                                         {
                                             const string actionUrl = "/Dispatcher/Render?incType=FakeMode&incIsModel=True&incView=~%2FFakeSerializeObject.cs";
                                             httpContext.Setup(r => r.Response.ApplyAppPathModifier(Pleasure.MockIt.IsStrong(actionUrl))).Returns(actionUrl);
                                             urlDispatcher.Model(new FakeModel { DecodeValue = "{{1}}", EncodeValue = HttpUtility.UrlEncode("{{1}}") })
                                                          .AsView("~/FakeSerializeObject.cs")
                                                          .ShouldEqual("/Dispatcher/Render?incType=FakeModel&incIsModel=True&incView=~%2FFakeSerializeObject.cs&EncodeValue=%7b%7b1%7d%7d&DecodeValue={{1}}");
                                         };

        It should_be_query_to_file = () =>
                                         {
                                             const string actionUrl = "/Dispatcher/QueryToFile?incType=FakeBytesQuery&incContentType=img&incFileDownloadName=file";
                                             httpContext.Setup(r => r.Response.ApplyAppPathModifier(Pleasure.MockIt.IsStrong(actionUrl))).Returns(actionUrl);
                                             urlDispatcher.Query(new FakeBytesQuery
                                                                     {
                                                                             DecodeValue = "{{1}}",
                                                                             EncodeValue = HttpUtility.UrlEncode("{{1}}"),
                                                                     })
                                                          .AsFile(incContentType: "img", incFileDownloadName: "file")
                                                          .ShouldEqual("/Dispatcher/QueryToFile?incType=FakeBytesQuery&incContentType=img&incFileDownloadName=file&EncodeValue=%7b%7b1%7d%7d&DecodeValue={{1}}");
                                         };

        It should_be_query_to_json = () =>
                                         {
                                             const string actionUrl = "/Dispatcher/Query?incType=FakeQuery";
                                             httpContext.Setup(r => r.Response.ApplyAppPathModifier(Pleasure.MockIt.IsStrong(actionUrl))).Returns(actionUrl);
                                             urlDispatcher.Query(new FakeQuery
                                                                     {
                                                                             DecodeValue = "{{1}}",
                                                                             EncodeValue = HttpUtility.UrlEncode("{{1}}"),
                                                                     })
                                                          .AsJson()
                                                          .ShouldEqual("/Dispatcher/Query?incType=FakeQuery&EncodeValue=%7b%7b1%7d%7d&DecodeValue={{1}}");
                                         };

        It should_be_query_to_string = () =>
                                           {
                                               var query = urlDispatcher.Query(new FakeQuery
                                                                                   {
                                                                                           DecodeValue = "{{1}}", EncodeValue = HttpUtility.UrlEncode("{{1}}"),
                                                                                   });
                                               query.ToString().ShouldEqual(query.AsJson());
                                           };

        It should_be_query_enable_validate_to_json = () =>
                                                         {
                                                             const string actionUrl = "/Dispatcher/Query?incType=FakeQuery&incValidate=True";
                                                             httpContext.Setup(r => r.Response.ApplyAppPathModifier(Pleasure.MockIt.IsStrong(actionUrl))).Returns(actionUrl);
                                                             urlDispatcher.Query(new FakeQuery())
                                                                          .EnableValidate()
                                                                          .AsJson()
                                                                          .ShouldEqual(actionUrl);
                                                         };

        It should_be_query_single_generic = () =>
                                                {
                                                    const string actionUrl = "/Dispatcher/Query?incType=FakeGenericQuery%601&incGeneric=IncEntityBase";
                                                    httpContext.Setup(r => r.Response.ApplyAppPathModifier(Pleasure.MockIt.IsStrong(actionUrl))).Returns(actionUrl);
                                                    urlDispatcher.Query(new FakeGenericQuery<IncEntityBase>())
                                                                 .AsJson()
                                                                 .ShouldEqual(actionUrl);
                                                };

        It should_be_query_multiple_generic = () =>
                                                  {
                                                      const string actionUrl = "/Dispatcher/Query?incType=FakeMultipleGenericQuery%602&incGeneric=IncEntityBase%2CString";
                                                      httpContext.Setup(r => r.Response.ApplyAppPathModifier(actionUrl)).Returns(actionUrl);
                                                      urlDispatcher.Query(new FakeMultipleGenericQuery<IncEntityBase, string>())
                                                                   .AsJson()
                                                                   .ShouldEqual(actionUrl); 
                                                  };

        It should_be_query_to_view = () =>
                                         {
                                             const string actionUrl = "/Dispatcher/Render?incType=FakeQuery&incView=~%2FFakeSerializeObject.cs";
                                             httpContext.Setup(r => r.Response.ApplyAppPathModifier(Pleasure.MockIt.IsStrong(actionUrl))).Returns(actionUrl);
                                             urlDispatcher.Query(new FakeQuery())
                                                          .AsView("~/FakeSerializeObject.cs")
                                                          .ShouldEqual(actionUrl);
                                         };
    }
}