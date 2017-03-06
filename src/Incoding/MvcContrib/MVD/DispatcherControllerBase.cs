namespace Incoding.MvcContrib.MVD
{
    #region << Using >>

    using System.Linq;
    using System.Web.Mvc;
    using Incoding.CQRS;
    using Incoding.Extensions;

    #endregion

    // ReSharper disable Mvc.ViewNotResolved
    // ReSharper disable MemberCanBeProtected.Global
    public class DispatcherControllerBase : IncControllerBase
    {
        #region Api Methods

        public virtual ActionResult Validate()
        {
            var parameter = dispatcher.Query(new GetMvdParameterQuery()
                                             {
                                                     Params = HttpContext.Request.Params
                                             });
            // ReSharper disable once UnusedVariable
            var instance = dispatcher.Query(new CreateByTypeQuery() { Type = parameter.Type, ControllerContext = this.ControllerContext, ModelState = ModelState });
            return ModelState.IsValid ? IncodingResult.Success() : IncodingResult.Error(ModelState);
        }

        public virtual ActionResult Query()
        {
            var parameter = dispatcher.Query(new GetMvdParameterQuery()
                                             {
                                                     Params = HttpContext.Request.Params
                                             });
            var query = dispatcher.Query(new CreateByTypeQuery() { Type = parameter.Type, ControllerContext = this.ControllerContext, ModelState = ModelState });

            if (parameter.IsValidate && !ModelState.IsValid)
                return IncodingResult.Error(ModelState);

            var composite = new CommandComposite((IMessage)query);
            return TryPush(commandComposite => dispatcher.Query(new MVDExecute(HttpContext) { Instance = composite }), composite, setting => setting.SuccessResult = () => IncodingResult.Success(composite.Parts[0].Result), isAjax: true);
        }

        public virtual ActionResult Render()
        {
            var parameter = dispatcher.Query(new GetMvdParameterQuery()
                                             {
                                                     Params = HttpContext.Request.Params
                                             });
            object model = null;
            if (!string.IsNullOrWhiteSpace(parameter.Type))
            {
                var instance = dispatcher.Query(new CreateByTypeQuery()
                                                {
                                                        Type = parameter.Type,
                                                        ControllerContext = ControllerContext,
                                                        ModelState = ModelState,
                                                        IsModel = parameter.IsModel
                                                });

                if (parameter.IsValidate && !ModelState.IsValid)
                    return IncodingResult.Error(ModelState);

                model = parameter.IsModel ? instance : dispatcher.Query(new MVDExecute(HttpContext) { Instance = new CommandComposite((IMessage)instance) });
            }

            ModelState.Clear();

            var isAjaxRequest = HttpContext.Request.IsAjaxRequest();
            return isAjaxRequest
                           ? (ActionResult)IncPartialView(parameter.View, model)
                           : Content(RenderToString(parameter.View, model));
        }

        public virtual ActionResult Push()
        {
            var parameter = dispatcher.Query(new GetMvdParameterQuery()
                                             {
                                                     Params = HttpContext.Request.Params
                                             });

            var commands = dispatcher.Query(new CreateByTypeQuery.AsCommands()
                                            {
                                                    IncTypes = parameter.Type,
                                                    ModelState = ModelState,
                                                    ControllerContext = ControllerContext,
                                                    IsComposite = parameter.IsCompositeArray
                                            });

            var composite = new CommandComposite(commands);
            return TryPush(commandComposite => dispatcher.Query(new MVDExecute(HttpContext) { Instance = composite }), composite, setting => setting.SuccessResult = () =>
                                                                                                                                                                     {
                                                                                                                                                                         var data = commands.Length == 1 ? commands[0].Result : commands.Select(r => r.Result);
                                                                                                                                                                         return IncodingResult.Success(data);
                                                                                                                                                                     });
        }

        public virtual ActionResult QueryToFile()
        {
            var parameter = dispatcher.Query(new GetMvdParameterQuery()
                                             {
                                                     Params = HttpContext.Request.Params
                                             });
            var instance = dispatcher.Query(new CreateByTypeQuery()
                                            {
                                                    Type = parameter.Type,
                                                    ControllerContext = ControllerContext,
                                                    ModelState = ModelState
                                            });
            var result = dispatcher.Query(new MVDExecute(HttpContext)
                                          {
                                                  Instance = new CommandComposite((IMessage)instance)
                                          });
            Guard.NotNull("result", result, "Result from query {0} is null but argument 'result' should be not null".F(parameter.Type));

            Response.AddHeader("X-Download-Options", "Open");
            return File((byte[])result, parameter.ContentType, parameter.FileDownloadName);
        }

        #endregion
    }
}