namespace Incoding.MvcContrib.MVD
{
    #region << Using >>

    using System.Text;
    using System.Web;
    using System.Web.Routing;
    using Incoding.CQRS;
    using Incoding.Extensions;

    #endregion

    public class QueryHttpHandler : IHttpHandler
    {
        public class QueryHandlerRoute : IRouteHandler
        {
            public IHttpHandler GetHttpHandler(RequestContext requestContext)
            {
                return new QueryHttpHandler();
            }
        }

        #region IHttpHandler Members

        public void ProcessRequest(HttpContext context)
        {
            var dispatcher = new DefaultDispatcher();
            var parameter = dispatcher.Query(new GetMvdParameterQuery()
                                             {
                                                     Params = context.Request.Params
                                             });
            context.Response.ContentType = "application/json";
            context.Response.ContentEncoding = Encoding.UTF8;
            var instanceForQuery = dispatcher.Query(new CreateByTypeQuery() { Type = parameter.Type });
            context.Response.Write(IncodingResult.Success(dispatcher.Query(new MVDExecute(new HttpContextWrapper(context)) { Instance = new CommandComposite((IMessage)instanceForQuery) })).ToJsonString());
        }

        public bool IsReusable { get { return false; } }

        public static readonly IRouteHandler Route = new QueryHandlerRoute();

        #endregion
    }
}