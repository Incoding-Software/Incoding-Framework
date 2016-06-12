namespace Incoding.MvcContrib.MVD
{
    #region << Using >>

    using System;
    using System.Collections.Specialized;
    using System.IO;
    using System.Text;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Incoding.CQRS;
    using Incoding.Extensions;

    #endregion

    public class DispatcherHttpHandler : IHttpHandler
    {
        #region Enums

        public enum Verb
        {
            Query,

            Render,

            Push
        }

        #endregion

        public static T CreateController<T>()
                where T : Controller, new()
        {
            // create a disconnected controller instance
            T controller = new T();
            controller.ControllerContext = new ControllerContext(new HttpContextWrapper(HttpContext.Current), new RouteData(), controller);
            return controller;
        }

        #region IHttpHandler Members

        public void ProcessRequest(HttpContext context)
        {
            var verb = (Verb)Enum.Parse(typeof(Verb), context.Request.Params["incVerb"]);
            var dispatcher = new DefaultDispatcher();
            var parameter = dispatcher.Query(new GetMvdParameterQuery()
                                             {
                                                     Params = context.Request.Params
                                             });
            context.Response.ContentType = "application/json";
            context.Response.ContentEncoding = Encoding.UTF8;

            switch (verb)
            {
                case Verb.Query:
                    var instanceForQuery = dispatcher.Query(new CreateByTypeQuery() { Type = parameter.Type });
                    context.Response.Write(IncodingResult.Success(dispatcher.Query(new ExecuteQuery() { Instance = instanceForQuery })).ToJsonString());
                    break;

                case Verb.Render:
                    object model = null;
                    if (!string.IsNullOrWhiteSpace(parameter.Type))
                    {
                        var instanceForRender = dispatcher.Query(new CreateByTypeQuery()
                                                                 {
                                                                         IsModel = parameter.IsModel,
                                                                         Type = parameter.Type,
                                                                 });
                        var baseType = instanceForRender.GetType().BaseType;

                        model = baseType.Name.EqualsWithInvariant("QueryBase`1") && !parameter.IsModel
                                        ? dispatcher.Query(new ExecuteQuery() { Instance = instanceForRender })
                                        : instanceForRender;
                    }

                    using (var sw = new StringWriter())
                    {
                        var viewResult = ViewEngines.Engines.FindPartialView(CreateController<DispatcherControllerBase>().ControllerContext, parameter.View);
                        var viewContext = new ViewContext(CreateController<DispatcherControllerBase>().ControllerContext, viewResult.View, new ViewDataDictionary(model), new TempDataDictionary(), sw);
                        viewResult.View.Render(viewContext, sw);
                        context.Response.Write(IncodingResult.Success(sw.GetStringBuilder().ToString()).ToJsonString());
                    }

                    break;
                case Verb.Push:
                    var instanceForPush = dispatcher.Query(new CreateByTypeQuery() { Type = parameter.Type });

                    var result = dispatcher.Query(new ExecuteQuery() { Instance = instanceForPush });
                    context.Response.Write(IncodingResult.Success(result).ToJsonString());
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public bool IsReusable { get; private set; }

        #endregion
    }

    public sealed class GetMvdParameterQuery : QueryBase<GetMvdParameterQuery.Response>
    {
        public NameValueCollection Params { get; set; }

        protected override Response ExecuteResult()
        {
            bool incIsModel;
            bool.TryParse(Params["incIsModel"], out incIsModel);

            bool onlyValidate;
            bool.TryParse(Params["IncOnlyValidate"], out onlyValidate);

            bool isValidate;
            bool.TryParse(Params["incValidate"], out isValidate);
            return new Response()
                   {
                           Type = Params["incType"] ?? Params["incTypes"],
                           IsModel = incIsModel,
                           View = Params["incView"],
                           OnlyValidate = onlyValidate,
                           IsValidate = isValidate
                   };
        }

        public class Response
        {
            public string Type { get; set; }

            public bool IsModel { get; set; }

            public string View { get; set; }

            public bool OnlyValidate { get; set; }

            public bool IsValidate { get; set; }
        }
    }
}