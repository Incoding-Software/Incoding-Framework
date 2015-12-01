namespace Incoding.MvcContrib.MVD
{
    #region << Using >>

    using System;
    using System.IO;
    using System.Text;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Incoding.Block.IoC;
    using Incoding.CQRS;
    using Incoding.Extensions;
    using Incoding.Maybe;

    #endregion

    public class DispatcherHttpHandler : IHttpHandler
    {
        public static T CreateController<T>(RouteData routeData = null)
            where T : Controller, new()
        {
            // create a disconnected controller instance
            T controller = new T();

            // get context wrapper from HttpContext if available
            HttpContextBase wrapper = null;
            if (HttpContext.Current != null)
                wrapper = new HttpContextWrapper(System.Web.HttpContext.Current);
            else
                throw new InvalidOperationException(
                    "Can't create Controller Context if no active HttpContext instance is available.");

            if (routeData == null)
                routeData = new RouteData();

            // add the controller routing if not existing
            if (!routeData.Values.ContainsKey("controller") && !routeData.Values.ContainsKey("Controller"))
                routeData.Values.Add("controller", controller.GetType().Name
                                                            .ToLower()
                                                            .Replace("controller", ""));

            controller.ControllerContext = new ControllerContext(wrapper, routeData, controller);
            return controller;
        }

        #region IHttpHandler Members

        public void ProcessRequest(HttpContext context)
        {
            var verb = (Verb)Enum.Parse(typeof(Verb), context.Request.Params["incVerb"]);
            string incTypes = context.Request.Params["incTypes"];
            bool incIsModel;
            bool.TryParse(context.Request.Params["incIsModel"], out incIsModel);
            var dispatcher = IoCFactory.Instance.TryResolve<IDispatcher>();
            context.Response.ContentType = "application/json";
            context.Response.ContentEncoding = Encoding.UTF8;

            switch (verb)
            {
                case Verb.Query:
                    var instanceForQuery = dispatcher.Query(new CreateByTypeQuery()
                                                            {
                                                                    Request = new HttpRequestWrapper(context.Request), 
                                                                    Type = incTypes
                                                            });

                    var result = dispatcher.GetType()
                                           .GetMethod("Query")
                                           .MakeGenericMethod(instanceForQuery.GetType().BaseType.With(r => r.GetGenericArguments()[0]))
                                           .Invoke(dispatcher, new[] { instanceForQuery, null });
                    context.Response.Write(IncodingResult.Success(result).ToJsonString());
                    break;

                case Verb.Render:
                    object model = null;
                    if (!string.IsNullOrWhiteSpace(incTypes))
                    {
                        var instanceForRender = dispatcher.Query(new CreateByTypeQuery()
                                                                 {
                                                                         IsModel = incIsModel, 
                                                                         Type = incTypes, 
                                                                         Request = new HttpRequestWrapper(context.Request)
                                                                 });
                        var baseType = instanceForRender.GetType().BaseType;

                        model = baseType.Name.EqualsWithInvariant("QueryBase`1") && !incIsModel
                                        ? dispatcher.GetType()
                                                    .GetMethod("Query")
                                                    .MakeGenericMethod(baseType.GetGenericArguments()[0])
                                                    .Invoke(dispatcher, new[] { instanceForRender, null })
                                        : instanceForRender;
                    }

                    //using (var sw = new StringWriter())
                    //{
                    //    var viewResult = ViewEngines.Engines.FindPartialView(CreateController<DispatcherControllerBase>().ControllerContext, context.Request.Params["incView"]);
                    //    var viewContext = new ViewContext(CreateController<DispatcherControllerBase>().ControllerContext, viewResult.View, new ViewDataDictionary(model), new TempDataDictionary(), sw);
                    //    viewResult.View.Render(viewContext, sw);
                    //    context.Response.Write(IncodingResult.Success(sw.GetStringBuilder().ToString()).ToJsonString());
                    //}

                    break;
                case Verb.Push:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public bool IsReusable { get; private set; }

        #endregion

        #region Enums

        public enum Verb
        {
            Query, 

            Render, 

            Push
        }

        #endregion
    }
}